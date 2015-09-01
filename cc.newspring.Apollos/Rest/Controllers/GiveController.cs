using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rock;
using Rock.Data;
using Rock.Financial;
using Rock.Model;
using Rock.Rest;
using Rock.Rest.Filters;
using Rock.Web.Cache;

namespace cc.newspring.Apollos.Rest.Controllers
{
    public class GiveController : ApiControllerBase
    {
        private static string gatewayName = "cc.newspring.CyberSource.Gateway";

        /// <summary>
        /// Saves a payment account through CyberSource with the specified parameters.
        /// </summary>
        /// <param name="giveParameters">The give parameters.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/SavePaymentAccount" )]
        public HttpResponseMessage SavePaymentAccount( [FromBody]PaymentParameters paymentParameters )
        {
            var rockContext = new RockContext();
            try
            {
                rockContext.WrapTransaction( () =>
                {
                    var person = GetExistingPerson( paymentParameters.PersonId, rockContext );

                    if ( person == null )
                    {
                        GenerateResponse( HttpStatusCode.BadRequest, "An existing person is required to save a payment" );
                    }

                    var gatewayComponent = GatewayContainer.GetComponent( gatewayName );

                    if ( gatewayComponent == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the gateway component" );
                    }

                    var financialGateway = new FinancialGatewayService( rockContext ).Queryable().FirstOrDefault( g => g.EntityTypeId == gatewayComponent.EntityType.Id );

                    if ( financialGateway == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the financial gateway" );
                    }

                    var locationId = CreateLocation( paymentParameters, rockContext );
                    var paymentDetail = CreatePaymentDetail( paymentParameters, person, locationId, rockContext );
                    var savedAccount = CreateSavedAccount( paymentParameters, paymentDetail, financialGateway, person, rockContext );
                    var paymentInfo = GetPaymentInfo( paymentParameters, person, rockContext, 0, paymentDetail );
                           
                    string errorMessage;
                    var transaction = gatewayComponent.Authorize( financialGateway, paymentInfo, out errorMessage );

                    if ( transaction == null || !string.IsNullOrWhiteSpace( errorMessage ) )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, errorMessage ?? "The gateway had a problem and/or did not create a transaction as expected" );
                    }

                    transaction.FinancialPaymentDetail = null;
                    transaction.SourceTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.FINANCIAL_SOURCE_TYPE_WEBSITE ) ).Id;
                    transaction.TransactionDateTime = RockDateTime.Now;
                    transaction.AuthorizedPersonAliasId = person.PrimaryAliasId;
                    transaction.AuthorizedPersonAlias = person.PrimaryAlias;
                    transaction.FinancialGatewayId = financialGateway.Id;
                    transaction.TransactionTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION ) ).Id;
                    transaction.FinancialPaymentDetailId = paymentDetail.Id;
                    savedAccount.TransactionCode = transaction.TransactionCode;

                    new FinancialTransactionService( rockContext ).Add( transaction );
                    rockContext.SaveChanges();

                    savedAccount.ReferenceNumber = gatewayComponent.GetReferenceNumber( transaction, out errorMessage );
                    rockContext.SaveChanges();
                } );
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch ( Exception exception )
            {
                var response = new HttpResponseMessage( HttpStatusCode.InternalServerError );
                response.Content = new StringContent( exception.Message );
                return response;
            }

            return new HttpResponseMessage( HttpStatusCode.NoContent );     
        }

        /// <summary>
        /// Schedules the giving.
        /// </summary>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/ScheduleGiving/Stop/{id}" )]
        public HttpResponseMessage StopScheduledGiving( int id )
        {
            var rockContext = new RockContext();
            try
            {
                rockContext.WrapTransaction( () =>
                {
                    var gatewayComponent = GatewayContainer.GetComponent( gatewayName );

                    if ( gatewayComponent == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the gateway component" );
                    }

                    var financialGateway = new FinancialGatewayService( rockContext ).Queryable().FirstOrDefault( g => g.EntityTypeId == gatewayComponent.EntityType.Id );

                    if ( financialGateway == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the financial gateway" );
                    }

                    var schedule = ( new FinancialScheduledTransactionService( rockContext ) ).Get( id );

                    if ( schedule == null )
                    {
                        GenerateResponse( HttpStatusCode.BadRequest, "No schedule with Id: " + id );
                    }

                    string errorMessage;
                    var error = gatewayComponent.CancelScheduledPayment( schedule, out errorMessage );

                    if ( error || errorMessage != null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, errorMessage ?? "There was an error with the gateway" );
                    }

                    schedule.IsActive = false;
                    rockContext.SaveChanges();
                } );
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch ( Exception exception )
            {
                var response = new HttpResponseMessage( HttpStatusCode.InternalServerError );
                response.Content = new StringContent( exception.Message );
                return response;
            }

            return new HttpResponseMessage( HttpStatusCode.OK );     
        }

        /// <summary>
        /// Schedules the giving.
        /// </summary>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/ScheduleGiving" )]
        public HttpResponseMessage ScheduleGiving( [FromBody]ScheduleParameters scheduleParameters )
        {
            var rockContext = new RockContext();

            try
            {
                rockContext.WrapTransaction( () =>
                {
                    var person = GetExistingPerson( scheduleParameters.PersonId, rockContext );

                    if ( person == null )
                    {
                        GenerateResponse( HttpStatusCode.BadRequest, "An existing person is required to schedule giving" );
                    }

                    var totalAmount = CalculateTotalAmount( scheduleParameters, rockContext );
                    var paymentSchedule = GetPaymentSchedule( scheduleParameters, person);
                    var gatewayComponent = GatewayContainer.GetComponent( gatewayName );

                    if ( gatewayComponent == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the gateway component" );
                    }

                    var financialGateway = new FinancialGatewayService( rockContext ).Queryable().FirstOrDefault( g => g.EntityTypeId == gatewayComponent.EntityType.Id );

                    if ( financialGateway == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the financial gateway" );
                    }

                    var savedAccount = GetExistingSavedAccount( scheduleParameters, person, rockContext );
                    FinancialPaymentDetail paymentDetail = null;
                    PaymentInfo paymentInfo = null;
                    int? locationId = null;

                    if ( savedAccount == null )
                    {
                        locationId = CreateLocation( scheduleParameters, rockContext );
                        paymentDetail = CreatePaymentDetail( scheduleParameters, person, locationId.Value, rockContext );
                        savedAccount = CreateSavedAccount( scheduleParameters, paymentDetail, financialGateway, person, rockContext );
                        paymentInfo = GetPaymentInfo( scheduleParameters, person, rockContext, totalAmount.Value, paymentDetail );
                    }
                    else
                    {
                        paymentDetail = savedAccount.FinancialPaymentDetail;
                        locationId = paymentDetail.BillingLocationId;
                        paymentInfo = savedAccount.GetReferencePayment();
                        UpdatePaymentInfoForSavedAccount(scheduleParameters, paymentInfo, person, rockContext, paymentDetail.BillingLocationId.Value, totalAmount.Value);
                    }

                    string errorMessage;
                    var schedule = gatewayComponent.AddScheduledPayment( financialGateway, paymentSchedule, paymentInfo, out errorMessage );

                    if ( schedule == null || !string.IsNullOrWhiteSpace( errorMessage ) )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, errorMessage ?? "The gateway had a problem and/or did not create a transaction as expected" );
                    }

                    schedule.TransactionFrequencyValueId = paymentSchedule.TransactionFrequencyValue.Id;

                    if ( person.PrimaryAliasId.HasValue )
                    {
                        schedule.AuthorizedPersonAliasId = person.PrimaryAliasId.Value;
                    }

                    schedule.FinancialPaymentDetail = paymentDetail;
                    schedule.FinancialPaymentDetail.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;

                    if ( paymentInfo.CreditCardTypeValue != null )
                    {
                        schedule.FinancialPaymentDetail.CreditCardTypeValueId = paymentInfo.CreditCardTypeValue.Id;
                    }

                    foreach ( var accountAmount in scheduleParameters.AmountDetails )
                    {
                        schedule.ScheduledTransactionDetails.Add( new FinancialScheduledTransactionDetail()
                        {
                            Amount = accountAmount.Amount,
                            AccountId = accountAmount.TargetAccountId
                        } );
                    }

                    new FinancialScheduledTransactionService( rockContext ).Add( schedule );
                    rockContext.SaveChanges();

                } );
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch ( Exception exception )
            {
                var response = new HttpResponseMessage( HttpStatusCode.InternalServerError );
                response.Content = new StringContent( exception.Message );
                return response;
            }

            return new HttpResponseMessage( HttpStatusCode.NoContent );           
        }

        /// <summary>
        /// Gives through CyberSource with the specified give parameters.
        /// </summary>
        /// <param name="giveParameters">The give parameters.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/Give" )]
        public HttpResponseMessage Give( [FromBody]GiveParameters giveParameters )
        {
            var rockContext = new RockContext();

            try
            {
                rockContext.WrapTransaction( () =>
                {
                    int? locationId = null;
                    FinancialPaymentDetail paymentDetail = null;
                    PaymentInfo paymentInfo = null;
                    FinancialPersonSavedAccount savedAccount = null;
                    bool newSavedAccount = true;

                    var gatewayComponent = GatewayContainer.GetComponent( gatewayName );

                    if ( gatewayComponent == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the gateway component" );
                    }

                    var financialGateway = new FinancialGatewayService( rockContext ).Queryable().FirstOrDefault( g => g.EntityTypeId == gatewayComponent.EntityType.Id );

                    if ( financialGateway == null )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the financial gateway" );
                    }

                    var totalAmount = CalculateTotalAmount( giveParameters, rockContext );
                    var person = GetExistingPerson( giveParameters.PersonId, rockContext );

                    if ( person == null )
                    {
                        // New person
                        locationId = CreateLocation( giveParameters, rockContext );
                        person = CreatePerson( giveParameters, locationId.Value, rockContext );
                    }
                    else
                    {
                        // Existing person
                        savedAccount = GetExistingSavedAccount( giveParameters, person, rockContext );

                        if ( savedAccount != null )
                        {
                            locationId = savedAccount.FinancialPaymentDetail.BillingLocationId;
                            newSavedAccount = false;
                        }
                    }

                    if ( !locationId.HasValue )
                    {
                        locationId = CreateLocation( giveParameters, rockContext );
                    }

                    if ( savedAccount == null )
                    {
                        paymentDetail = CreatePaymentDetail( giveParameters, person, locationId.Value, rockContext );
                        savedAccount = CreateSavedAccount( giveParameters, paymentDetail, financialGateway, person, rockContext );
                        newSavedAccount = true;
                        paymentInfo = GetPaymentInfo( giveParameters, person, rockContext, totalAmount.Value, paymentDetail );
                    }
                    else
                    {
                        paymentDetail = savedAccount.FinancialPaymentDetail;
                        locationId = paymentDetail.BillingLocationId;
                        paymentInfo = savedAccount.GetReferencePayment();
                        UpdatePaymentInfoForSavedAccount( giveParameters, paymentInfo, person, rockContext, locationId.Value, totalAmount.Value );
                    }

                    string errorMessage;
                    var transaction = gatewayComponent.Charge( financialGateway, paymentInfo, out errorMessage );

                    if ( transaction == null || !string.IsNullOrWhiteSpace( errorMessage ) )
                    {
                        GenerateResponse( HttpStatusCode.InternalServerError, errorMessage ?? "The gateway had a problem and/or did not create a transaction as expected" );
                    }

                    transaction.FinancialPaymentDetail = null;
                    transaction.SourceTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.FINANCIAL_SOURCE_TYPE_WEBSITE ) ).Id;
                    transaction.TransactionDateTime = RockDateTime.Now;
                    transaction.AuthorizedPersonAliasId = person.PrimaryAliasId;
                    transaction.AuthorizedPersonAlias = person.PrimaryAlias;
                    transaction.FinancialGatewayId = financialGateway.Id;
                    transaction.TransactionTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION ) ).Id;
                    transaction.FinancialPaymentDetailId = paymentDetail.Id;
                    savedAccount.TransactionCode = transaction.TransactionCode;

                    foreach ( var accountAmount in giveParameters.AmountDetails )
                    {
                        transaction.TransactionDetails.Add( new FinancialTransactionDetail()
                        {
                            Amount = accountAmount.Amount,
                            AccountId = accountAmount.TargetAccountId
                        } );
                    }

                    new FinancialTransactionService( rockContext ).Add( transaction );
                    rockContext.SaveChanges();

                    if ( newSavedAccount )
                    {
                        var newReferenceNumber = gatewayComponent.GetReferenceNumber( transaction, out errorMessage );
                        savedAccount.ReferenceNumber = newReferenceNumber;
                    }

                    rockContext.SaveChanges();
                } );
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch ( Exception exception )
            {
                var response = new HttpResponseMessage( HttpStatusCode.InternalServerError );
                response.Content = new StringContent( exception.Message );
                return response;
            }

            return new HttpResponseMessage( HttpStatusCode.NoContent ); 
        }

        private void GenerateResponse( HttpStatusCode code, string message = null )
        {
            var response = new HttpResponseMessage( code );

            if ( !string.IsNullOrWhiteSpace( message ) )
            {
                response.Content = new StringContent( message );
            }

            throw new HttpResponseException(response);
        }

        private string Mask( string unmasked, int charsToShow = 4, char maskChar = '*' )
        {
            var lengthOfUnmasked = unmasked.Length;
            var maxCharsToShow = lengthOfUnmasked / 2;
            charsToShow = charsToShow > maxCharsToShow ? maxCharsToShow : charsToShow;
            var charsToMask = lengthOfUnmasked - charsToShow;

            if ( lengthOfUnmasked <= charsToShow )
            {
                return unmasked;
            }

            var mask = string.Empty.PadLeft( charsToMask, maskChar );
            var shown = unmasked.Substring( unmasked.Length - charsToShow );
            return string.Concat( mask, shown );
        }

        private Person GetExistingPerson( int? personId, RockContext rockContext )
        {
            if ( !personId.HasValue )
            {
                return null;
            }

            var person = new PersonService( rockContext ).Get( personId.Value );

            if ( person == null )
            {
                GenerateResponse(HttpStatusCode.BadRequest, "The PersonId passed is invalid");
                return null;
            }

            return person;
        }

        private FinancialPersonSavedAccount GetExistingSavedAccount( GiveParameters giveParameters, Person person, RockContext rockContext )
        {
            if ( !giveParameters.SourceAccountId.HasValue )
            {
                return null;
            }

            var savedAccount = new FinancialPersonSavedAccountService( rockContext ).Get( giveParameters.SourceAccountId.Value );

            if ( savedAccount == null )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "The SourceAccountId passed is invalid" );
                return null;
            }

            if ( !savedAccount.PersonAliasId.HasValue )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "The SourceAccount doesn't belong to anyone" );
                return null;
            }

            if ( person.Aliases.All( a => a.Id != savedAccount.PersonAliasId.Value ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "The SourceAccount doesn't belong to the passed PersonId" );
                return null;
            }

            return savedAccount;
        }

        private FinancialPersonSavedAccount CreateSavedAccount( PaymentParameters parameters, FinancialPaymentDetail paymentDetail, FinancialGateway financialGateway, Person person, RockContext rockContext) {
            var lastFour = paymentDetail.AccountNumberMasked.Substring(paymentDetail.AccountNumberMasked.Length - 4);
            var name = string.Empty;

            if ( parameters.AccountType.ToLower() != "credit" )
            {
                if ( string.IsNullOrWhiteSpace( parameters.RoutingNumber ) )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "RoutingNumber is required for ACH transactions" );
                    return null;
                }

                if ( string.IsNullOrWhiteSpace( parameters.AccountNumber ) )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "AccountNumber is required" );
                    return null;
                }

                name = "Bank card ***" + lastFour;
                var bankAccountService = new FinancialPersonBankAccountService( rockContext );
                var accountNumberSecured = FinancialPersonBankAccount.EncodeAccountNumber( parameters.RoutingNumber, parameters.AccountNumber );
                var bankAccount = bankAccountService.Queryable().Where( a =>
                    a.AccountNumberSecured == accountNumberSecured &&
                    a.PersonAliasId == person.PrimaryAliasId.Value ).FirstOrDefault();

                if ( bankAccount == null )
                {
                    bankAccount = new FinancialPersonBankAccount();
                    bankAccount.PersonAliasId = person.PrimaryAliasId.Value;
                    bankAccount.AccountNumberMasked = paymentDetail.AccountNumberMasked;
                    bankAccount.AccountNumberSecured = accountNumberSecured;
                    bankAccountService.Add( bankAccount );
                }
            }
            else
            {
                name = "Credit card ***" + lastFour;
            }

            var savedAccount = new FinancialPersonSavedAccount { 
                PersonAliasId = person.PrimaryAliasId,
                FinancialGatewayId = financialGateway.Id,
                Name = name,
                FinancialPaymentDetailId = paymentDetail.Id
            };

            new FinancialPersonSavedAccountService(rockContext).Add( savedAccount );
            rockContext.SaveChanges();
            return savedAccount;
        }

        private int CreateLocation( PaymentParameters parameters, RockContext rockContext )
        {
            if ( string.IsNullOrWhiteSpace( parameters.Street1 ) ||
                string.IsNullOrWhiteSpace( parameters.City ) ||
                string.IsNullOrWhiteSpace( parameters.State ) ||
                string.IsNullOrWhiteSpace( parameters.PostalCode ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Street1, City, State, and PostalCode are required" );
            }

            if ( parameters.State == null || parameters.State.Length != 2 )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "State must be a 2 letter string" );
            }

            var location = new Location
            {
                Street1 = parameters.Street1,
                Street2 = parameters.Street2,
                City = parameters.City,
                State = parameters.State,
                PostalCode = parameters.PostalCode,
                Country = parameters.Country ?? "USA"
            };

            new LocationService( rockContext ).Add( location );
            rockContext.SaveChanges();
            return location.Id;
        }

        private FinancialPaymentDetail CreatePaymentDetail( PaymentParameters parameters, Person person, int billingLocationId, RockContext rockContext )
        {
            if ( string.IsNullOrWhiteSpace( parameters.AccountNumber ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "AccountNumber is required" );
                return null;
            }

            if ( string.IsNullOrWhiteSpace( parameters.AccountType ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "AccountType is required" );
                return null;
            }

            var accountType = parameters.AccountType.ToLower();
            var allowedTypes = new String[] { "checking", "savings", "credit" };

            if (!allowedTypes.Contains(accountType) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "AccountType must be one of " + string.Join(", ", allowedTypes) );
                return null;
            }

            var maskedAccountNumber = Mask( parameters.AccountNumber );
            var nameOnCard = ( parameters.FirstName ?? person.FirstName ) + " " + ( parameters.LastName ?? person.LastName );
            
            var paymentDetail = new FinancialPaymentDetail
            {
                AccountNumberMasked = maskedAccountNumber,
                NameOnCardEncrypted = Rock.Security.Encryption.EncryptString(nameOnCard),
                BillingLocationId = billingLocationId
            };

            if ( parameters.AccountType.ToLower() == "credit" )
            {
                paymentDetail.ExpirationMonthEncrypted = Rock.Security.Encryption.EncryptString( parameters.ExpirationMonth.ToString() );
                paymentDetail.ExpirationYearEncrypted = Rock.Security.Encryption.EncryptString( parameters.ExpirationYear.ToString() );
            }

            new FinancialPaymentDetailService( rockContext ).Add( paymentDetail );
            rockContext.SaveChanges();
            return paymentDetail;
        }

        private PaymentSchedule GetPaymentSchedule( ScheduleParameters scheduleParameters, Person person )
        {
            if ( !scheduleParameters.StartDate.HasValue )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Schedule must contain a StartDate" );
                return null;
            }

            var timeSpan = DateTime.Now - scheduleParameters.StartDate.Value;

            if ( timeSpan > TimeSpan.FromDays( 0 ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Schedule must contain a StartDate that occurs in the future" );
                return null;
            }

            if ( !scheduleParameters.FrequencyValueGuid.HasValue )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Schedule must contain a FrequencyValueGuid" );
                return null;
            }

            var frequency = DefinedValueCache.Read( scheduleParameters.FrequencyValueGuid.Value );

            if ( frequency == null )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Schedule must contain a valid FrequencyValueGuid" );
                return null;
            }

            return new PaymentSchedule { 
                PersonId = person.Id,
                StartDate = scheduleParameters.StartDate.Value,
                TransactionFrequencyValue = frequency
            };
        }

        private PaymentInfo GetPaymentInfo( PaymentParameters parameters, Person person, RockContext rockContext, decimal totalAmount, FinancialPaymentDetail paymentDetail )
        {
            PaymentInfo paymentInfo = null;

            if ( parameters.AccountType.ToLower() == "credit" )
            {
                if ( parameters.ExpirationMonth < 1 || parameters.ExpirationMonth > 12 )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "ExpirationMonth is required and must be between 1 and 12 for credit transactions" );
                }

                var currentDate = DateTime.Now;
                var maxYear = currentDate.Year + 30;

                if ( parameters.ExpirationYear < currentDate.Year || parameters.ExpirationYear > maxYear )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, string.Format( "ExpirationYear is required and must be between {0} and {1} for credit transactions", currentDate.Year, maxYear ) );
                }

                if ( parameters.ExpirationYear <= currentDate.Year && parameters.ExpirationMonth < currentDate.Month )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "The ExpirationMonth and ExpirationYear combination must not have already elapsed for credit transactions" );
                }

                if ( string.IsNullOrWhiteSpace( parameters.Street1 ) ||
                    string.IsNullOrWhiteSpace( parameters.City ) ||
                    string.IsNullOrWhiteSpace( parameters.State ) ||
                    string.IsNullOrWhiteSpace( parameters.PostalCode ) )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "Street1, City, State, and PostalCode are required for credit transactions" );
                }

                paymentInfo = new CreditCardPaymentInfo()
                {
                    Number = parameters.AccountNumber,
                    Code = parameters.CCV ?? string.Empty,
                    ExpirationDate = new DateTime( parameters.ExpirationYear, parameters.ExpirationMonth, 1 ),
                    BillingStreet1 = parameters.Street1 ?? string.Empty,
                    BillingStreet2 = parameters.Street2 ?? string.Empty,
                    BillingCity = parameters.City ?? string.Empty,
                    BillingState = parameters.State ?? string.Empty,
                    BillingPostalCode = parameters.PostalCode ?? string.Empty,
                    BillingCountry = parameters.Country ?? "USA"
                };                
            }
            else
            {
                if ( string.IsNullOrWhiteSpace( parameters.RoutingNumber ) )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "RoutingNumber is required for ACH transactions" );
                    return null;
                }

                paymentInfo = new ACHPaymentInfo()
                {
                    BankRoutingNumber = parameters.RoutingNumber,
                    BankAccountNumber = parameters.AccountNumber,
                    AccountType = parameters.AccountType.ToLower() == "checking" ? BankAccountType.Checking : BankAccountType.Savings
                };
            }
            
            paymentInfo.Amount = totalAmount;
            paymentInfo.FirstName = parameters.FirstName ?? person.FirstName;
            paymentInfo.LastName = parameters.LastName ?? person.LastName;
            paymentInfo.Email = parameters.Email ?? person.Email;
            paymentInfo.Phone = parameters.PhoneNumber ?? string.Empty;
            paymentInfo.Street1 = parameters.Street1 ?? string.Empty;
            paymentInfo.Street2 = parameters.Street2 ?? string.Empty;
            paymentInfo.City = parameters.City ?? string.Empty;
            paymentInfo.State = parameters.State ?? string.Empty;
            paymentInfo.PostalCode = parameters.PostalCode ?? string.Empty;
            paymentInfo.Country = parameters.Country ?? "USA";

            if ( paymentInfo.CreditCardTypeValue != null )
            {
                paymentDetail.CreditCardTypeValueId = paymentInfo.CreditCardTypeValue.Id;
            }

            if ( paymentInfo.CurrencyTypeValue != null )
            {
                paymentDetail.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;
            }

            return paymentInfo;
        }

        private void UpdatePaymentInfoForSavedAccount( GiveParameters giveParameters, PaymentInfo paymentInfo, Person person, RockContext rockContext, int billingLocationId, decimal totalAmount )
        {
            var billingLocation = new LocationService( rockContext ).Get(billingLocationId);

            paymentInfo.FirstName = giveParameters.FirstName ?? person.FirstName;
            paymentInfo.LastName = giveParameters.LastName ?? person.LastName;
            paymentInfo.Email = giveParameters.Email ?? person.Email;
            paymentInfo.Phone = giveParameters.PhoneNumber ?? string.Empty;
            paymentInfo.Street1 = giveParameters.Street1 ?? billingLocation.Street1;
            paymentInfo.Street2 = giveParameters.Street2 ?? billingLocation.Street2;
            paymentInfo.City = giveParameters.City ?? billingLocation.City;
            paymentInfo.State = giveParameters.State ?? billingLocation.State;
            paymentInfo.PostalCode = giveParameters.PostalCode ?? billingLocation.PostalCode;
            paymentInfo.Country = giveParameters.Country ?? billingLocation.Country;
            paymentInfo.Amount = totalAmount;
        }

        private decimal? CalculateTotalAmount( GiveParameters giveParameters, RockContext rockContext )
        {
            var totalAmount = 0m;

            if ( giveParameters.AmountDetails == null || giveParameters.AmountDetails.Length == 0 )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "AmountDetails are required" );
                return null;
            }

            foreach ( var accountAmount in giveParameters.AmountDetails )
            {
                if ( accountAmount.Amount < 1m )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "AmountDetails/Amount is required and must be greater than or equal to 1" );
                    return null;
                }
                if ( accountAmount.TargetAccountId == 0 )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "AmountDetails/TargetAccountId is required" );
                    return null;
                }
                if ( new FinancialAccountService( rockContext ).Get( accountAmount.TargetAccountId ) == null )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "AmountDetails/TargetAccountId must be an existing account's id" );
                    return null;
                }

                totalAmount += accountAmount.Amount;
            }

            if ( totalAmount < 1 )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Total gift must be at least $1" );
                return null;
            }

            return totalAmount;
        }

        private Person CreatePerson( GiveParameters giveParameters, int locationId, RockContext rockContext )
        {
            if ( string.IsNullOrWhiteSpace( giveParameters.Email ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Email is required" );
                return null;
            }

            if ( !giveParameters.Email.IsValidEmail() )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "Email must be valid" );
                return null;
            }

            if ( string.IsNullOrWhiteSpace( giveParameters.FirstName ) || string.IsNullOrWhiteSpace( giveParameters.LastName ) )
            {
                GenerateResponse( HttpStatusCode.BadRequest, "FirstName and LastName are required" );
                return null;
            }

            var person = new Person()
            {
                Guid = giveParameters.PersonGuid.HasValue ? giveParameters.PersonGuid.Value : Guid.NewGuid(),
                FirstName = giveParameters.FirstName,
                LastName = giveParameters.LastName,
                IsEmailActive = true,
                Email = giveParameters.Email,
                EmailPreference = EmailPreference.EmailAllowed,
                RecordTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON.AsGuid() ).Id,
                ConnectionStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_PARTICIPANT ).Id,
                RecordStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE ).Id
            };

            // Create Person/Family
            var familyGroup = PersonService.SaveNewPerson( person, rockContext, giveParameters.CampusId, false );

            if ( !string.IsNullOrWhiteSpace( giveParameters.PhoneNumber ) )
            {
                person.PhoneNumbers.Add( new PhoneNumber()
                {
                    Number = giveParameters.PhoneNumber,
                    NumberTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME ) ).Id
                } );
            }

            familyGroup.GroupLocations.Add( new GroupLocation()
            {
                GroupLocationTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME ) ).Id,
                LocationId = locationId
            } );

            if ( giveParameters.UserId.HasValue )
            {
                var user = new UserLoginService( rockContext ).Get(giveParameters.UserId.Value);

                if ( !user.PersonId.HasValue )
                {
                    user.PersonId = person.Id;
                }
            }

            rockContext.SaveChanges();
            return person;
        }
    }
}
