using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Rock.Data;
using Rock.Financial;
using Rock.Rest;
using Rock.Rest.Filters;

namespace cc.newspring.Apollos.Rest.Controllers
{
    public class GiveController : ApiControllerBase
    {
        private static string gatewayName = "Rock.CyberSource.Gateway";

        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/Give" )]
        public HttpResponseMessage Give( [FromBody]GiveParameters giveParameters )
        {
            var rockContext = new RockContext();
            var gateway = GatewayContainer.GetComponent( gatewayName );

            if ( gateway == null )
            {
                var response = new HttpResponseMessage( HttpStatusCode.InternalServerError );
                response.Content = new StringContent( "There was a problem creating the payment gateway information" );
                return response;
            }

            Person person = GetPerson( true );
            if ( person == null )
            {
                errorMessage = "There was a problem creating the person information";
                return false;
            }

            if ( !person.PrimaryAliasId.HasValue )
            {
                errorMessage = "There was a problem creating the person's primary alias";
                return false;
            }

            PaymentInfo paymentInfo = GetPaymentInfo();
            if ( paymentInfo == null )
            {
                errorMessage = "There was a problem creating the payment information";
                return false;
            }
            else
            {
                paymentInfo.FirstName = person.FirstName;
                paymentInfo.LastName = person.LastName;
            }

            if ( paymentInfo.CreditCardTypeValue != null )
            {
                CreditCardTypeValueId = paymentInfo.CreditCardTypeValue.Id;
            }

            PaymentSchedule schedule = GetSchedule();
            if ( schedule != null )
            {
                schedule.PersonId = person.Id;

                var scheduledTransaction = gateway.AddScheduledPayment( schedule, paymentInfo, out errorMessage );
                if ( scheduledTransaction != null )
                {
                    scheduledTransaction.TransactionFrequencyValueId = schedule.TransactionFrequencyValue.Id;
                    scheduledTransaction.AuthorizedPersonAliasId = person.PrimaryAliasId.Value;
                    // TODO: SHOULD BE 255
                    scheduledTransaction.GatewayEntityTypeId = gateway.Id;
                    scheduledTransaction.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;
                    scheduledTransaction.CreditCardTypeValueId = CreditCardTypeValueId;

                    var changeSummary = new StringBuilder();
                    changeSummary.AppendFormat( "{0} starting {1}", schedule.TransactionFrequencyValue.Value, schedule.StartDate.ToShortDateString() );
                    changeSummary.AppendLine();
                    changeSummary.Append( paymentInfo.CurrencyTypeValue.Value );
                    if ( paymentInfo.CreditCardTypeValue != null )
                    {
                        changeSummary.AppendFormat( " - {0}", paymentInfo.CreditCardTypeValue.Value );
                    }
                    changeSummary.AppendFormat( " {0}", paymentInfo.MaskedNumber );
                    changeSummary.AppendLine();

                    foreach ( var account in SelectedAccounts.Where( a => a.Amount > 0 ) )
                    {
                        var transactionDetail = new FinancialScheduledTransactionDetail();
                        transactionDetail.Amount = account.Amount;
                        transactionDetail.AccountId = account.Id;
                        scheduledTransaction.ScheduledTransactionDetails.Add( transactionDetail );
                        changeSummary.AppendFormat( "{0}: {1:C2}", account.Name, account.Amount );
                        changeSummary.AppendLine();
                    }

                    var transactionService = new FinancialScheduledTransactionService( rockContext );
                    transactionService.Add( scheduledTransaction );
                    rockContext.SaveChanges();

                    // Add a note about the change
                    var noteTypeService = new NoteTypeService( rockContext );
                    var noteType = noteTypeService.Get( scheduledTransaction.TypeId, "Note" );

                    var noteService = new NoteService( rockContext );
                    var note = new Note();
                    note.NoteTypeId = noteType.Id;
                    note.EntityId = scheduledTransaction.Id;
                    note.Caption = "Created Transaction";
                    note.Text = changeSummary.ToString();
                    noteService.Add( note );

                    rockContext.SaveChanges();

                    ScheduleId = scheduledTransaction.GatewayScheduleId;
                    TransactionCode = scheduledTransaction.TransactionCode;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var transaction = gateway.Charge( paymentInfo, out errorMessage );
                if ( transaction != null )
                {
                    transaction.TransactionDateTime = RockDateTime.Now;
                    transaction.AuthorizedPersonAliasId = person.PrimaryAliasId;
                    transaction.GatewayEntityTypeId = gateway.TypeId;
                    transaction.TransactionTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION ) ).Id;
                    transaction.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;
                    transaction.CreditCardTypeValueId = CreditCardTypeValueId;

                    Guid sourceGuid = Guid.Empty;
                    if ( Guid.TryParse( GetAttributeValue( "Source" ), out sourceGuid ) )
                    {
                        transaction.SourceTypeValueId = DefinedValueCache.Read( sourceGuid ).Id;
                    }

                    foreach ( var account in SelectedAccounts.Where( a => a.Amount > 0 ) )
                    {
                        var transactionDetail = new FinancialTransactionDetail();
                        transactionDetail.Amount = account.Amount;
                        transactionDetail.AccountId = account.Id;
                        transaction.TransactionDetails.Add( transactionDetail );
                    }

                    var batchService = new FinancialBatchService( rockContext );

                    // Get the batch
                    var batch = batchService.Get(
                        GetAttributeValue( "BatchNamePrefix" ),
                        paymentInfo.CurrencyTypeValue,
                        paymentInfo.CreditCardTypeValue,
                        transaction.TransactionDateTime.Value,
                        gateway.BatchTimeOffset );

                    batch.ControlAmount += transaction.TotalAmount;

                    transaction.BatchId = batch.Id;
                    batch.Transactions.Add( transaction );
                    rockContext.SaveChanges();

                    TransactionCode = transaction.TransactionCode;
                }
                else
                {
                    return false;
                }
            }

            tdTransactionCodeReceipt.Description = TransactionCode;
            tdTransactionCodeReceipt.Visible = !string.IsNullOrWhiteSpace( TransactionCode );

            tdScheduleId.Description = ScheduleId;
            tdScheduleId.Visible = !string.IsNullOrWhiteSpace( ScheduleId );

            tdNameReceipt.Description = paymentInfo.FullName;
            tdPhoneReceipt.Description = paymentInfo.Phone;
            tdEmailReceipt.Description = paymentInfo.Email;
            tdAddressReceipt.Description = string.Format( "{0} {1}, {2} {3}", paymentInfo.Street1, paymentInfo.City, paymentInfo.State, paymentInfo.PostalCode );

            rptAccountListReceipt.DataSource = SelectedAccounts.Where( a => a.Amount != 0 );
            rptAccountListReceipt.DataBind();

            tdTotalReceipt.Description = paymentInfo.Amount.ToString( "C" );

            tdPaymentMethodReceipt.Description = paymentInfo.CurrencyTypeValue.Description;
            tdAccountNumberReceipt.Description = paymentInfo.MaskedNumber;
            tdWhenReceipt.Description = schedule != null ? schedule.ToString() : "Today";

            // If there was a transaction code returned and this was not already created from a previous saved account,
            // show the option to save the account.
            if ( !( paymentInfo is ReferencePaymentInfo ) && !string.IsNullOrWhiteSpace( TransactionCode ) )
            {
                cbSaveAccount.Visible = true;
                pnlSaveAccount.Visible = true;
                txtSaveAccount.Visible = true;

                // If current person does not have a login, have them create a username and password
                phCreateLogin.Visible = !new UserLoginService( rockContext ).GetByPersonId( person.Id ).Any();
            }
            else
            {
                pnlSaveAccount.Visible = false;
            }

            return new HttpResponseMessage( HttpStatusCode.NoContent );
        }
    }
}