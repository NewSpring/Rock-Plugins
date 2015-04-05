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
        private static string gatewayName = "Rock.CyberSource.Gateway";

        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/Give" )]
        public HttpResponseMessage Give( [FromBody]GiveParameters giveParameters )
        {
            if ( string.IsNullOrWhiteSpace( giveParameters.PhoneNumber ) )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "PhoneNumber is required" );
            }

            if ( string.IsNullOrWhiteSpace( giveParameters.Email ) )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "Email is required" );
            }

            if ( string.IsNullOrWhiteSpace( giveParameters.FirstName ) ||
                string.IsNullOrWhiteSpace( giveParameters.LastName ) )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "FirstName and LastName are required" );
            }

            if ( string.IsNullOrWhiteSpace( giveParameters.AccountNumber ) )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "AccountNumber is required" );
            }

            if ( string.IsNullOrWhiteSpace( giveParameters.AccountType ) )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "AccountType is required and must be one of checking, savings, or credit" );
            }

            if ( giveParameters.State != null && giveParameters.State.Length != 2 )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "State must be a 2 letter string if supplied" );
            }

            switch ( giveParameters.AccountType.ToLower() )
            {
                case "checking":
                case "savings":
                    if ( string.IsNullOrWhiteSpace( giveParameters.RoutingNumber ) )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, "RoutingNumber is required for ACH transactions" );
                    }
                    break;

                case "credit":
                    if ( string.IsNullOrWhiteSpace( giveParameters.CCV ) )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, "CCV is required for credit transactions" );
                    }
                    if ( giveParameters.ExpirationMonth < 1 || giveParameters.ExpirationMonth > 12 )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, "ExpirationMonth is required and must be between 1 and 12 for credit transactions" );
                    }
                    var currentDate = DateTime.Now;
                    var maxYear = currentDate.Year + 30;
                    if ( giveParameters.ExpirationYear < currentDate.Year || giveParameters.ExpirationYear > maxYear )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, string.Format( "ExpirationYear is required and must be between {0} and {1} for credit transactions", currentDate.Year, maxYear ) );
                    }
                    if ( giveParameters.ExpirationYear <= currentDate.Year && giveParameters.ExpirationMonth < currentDate.Month )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, "The ExpirationMonth and ExpirationYear combination must not have already elapsed for credit transactions" );
                    }
                    if ( string.IsNullOrWhiteSpace( giveParameters.Street1 ) ||
                        string.IsNullOrWhiteSpace( giveParameters.City ) ||
                        string.IsNullOrWhiteSpace( giveParameters.State ) ||
                        string.IsNullOrWhiteSpace( giveParameters.PostalCode ) )
                    {
                        return GenerateResponse( HttpStatusCode.BadRequest, "Street1, City, State, and PostalCode are required for credit transactions" );
                    }
                    break;

                default:
                    return GenerateResponse( HttpStatusCode.BadRequest, "AccountType is required and must be one of checking, savings, or credit" );
            }

            var totalAmount = 0m;
            var rockContext = new RockContext();

            foreach ( var accountAmount in giveParameters.Amounts )
            {
                if ( accountAmount.Amount < 0m )
                {
                    return GenerateResponse( HttpStatusCode.BadRequest, "Amounts/Amount is required and must be greater than or equal to 0" );
                }
                if ( accountAmount.AccountId == 0 )
                {
                    return GenerateResponse( HttpStatusCode.BadRequest, "Amounts/AccountId is required" );
                }
                if ( new FinancialAccountService( rockContext ).Get( accountAmount.AccountId ) == null )
                {
                    return GenerateResponse( HttpStatusCode.BadRequest, "Amounts/AccountId must be an existing account's id" );
                }

                totalAmount += accountAmount.Amount;
            }

            if ( totalAmount < 1m )
            {
                return GenerateResponse( HttpStatusCode.BadRequest, "Amounts are required and the sum of them must be greater than or equal to 1" );
            }

            var gateway = GatewayContainer.GetComponent( gatewayName );

            if ( gateway == null )
            {
                return GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating the payment gateway information" );
            }

            PaymentInfo paymentInfo = null;

            if ( giveParameters.AccountType.ToLower() == "credit" )
            {
                paymentInfo = new CreditCardPaymentInfo()
                {
                    Number = giveParameters.AccountNumber,
                    Code = giveParameters.CCV,
                    ExpirationDate = new DateTime( giveParameters.ExpirationYear, giveParameters.ExpirationMonth, 1 ),
                    BillingStreet1 = giveParameters.Street1,
                    BillingStreet2 = giveParameters.Street2,
                    BillingCity = giveParameters.City,
                    BillingState = giveParameters.State,
                    BillingPostalCode = giveParameters.PostalCode,
                    BillingCountry = giveParameters.Country
                };
            }
            else
            {
                paymentInfo = new ACHPaymentInfo()
                {
                    BankRoutingNumber = giveParameters.RoutingNumber,
                    BankAccountNumber = giveParameters.AccountNumber,
                    AccountType = giveParameters.AccountType.ToLower() == "checking" ? BankAccountType.Checking : BankAccountType.Savings
                };
            }

            paymentInfo.Street1 = giveParameters.Street1;
            paymentInfo.Street2 = giveParameters.Street2;
            paymentInfo.City = giveParameters.City;
            paymentInfo.State = giveParameters.State;
            paymentInfo.PostalCode = giveParameters.PostalCode;
            paymentInfo.Country = giveParameters.Country;

            paymentInfo.Amount = totalAmount;
            paymentInfo.FirstName = giveParameters.FirstName;
            paymentInfo.LastName = giveParameters.LastName;
            paymentInfo.Email = giveParameters.Email;
            paymentInfo.Phone = giveParameters.PhoneNumber;

            Person person = null;

            if ( giveParameters.PersonId.HasValue )
            {
                person = new PersonService( rockContext ).Get( giveParameters.PersonId.Value );

                if ( person == null )
                {
                    return GenerateResponse( HttpStatusCode.BadRequest, "The PersonId did not resolve to an existing person record" );
                }
            }
            else
            {
                person = CreatePerson( giveParameters );

                if ( person == null )
                {
                    return GenerateResponse( HttpStatusCode.InternalServerError, "There was a problem creating a person with the supplied details" );
                }
            }

            string errorMessage;
            var transaction = gateway.Charge( paymentInfo, out errorMessage );

            if ( transaction == null || !string.IsNullOrWhiteSpace( errorMessage ) )
            {
                return GenerateResponse( HttpStatusCode.InternalServerError, errorMessage ?? "The transactions could not be created" );
            }

            transaction.TransactionDateTime = RockDateTime.Now;
            transaction.AuthorizedPersonAliasId = person.PrimaryAliasId;
            transaction.GatewayEntityTypeId = gateway.TypeId;
            transaction.TransactionTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION ) ).Id;
            transaction.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;

            foreach ( var accountAmount in giveParameters.Amounts )
            {
                transaction.TransactionDetails.Add( new FinancialTransactionDetail()
                {
                    Amount = accountAmount.Amount,
                    AccountId = accountAmount.AccountId
                } );
            }

            new FinancialTransactionService( rockContext ).Add( transaction );
            rockContext.SaveChanges();
            return new HttpResponseMessage( HttpStatusCode.NoContent );
        }

        private Person CreatePerson( GiveParameters giveParameters )
        {
            var person = new Person()
            {
                FirstName = giveParameters.FirstName,
                LastName = giveParameters.LastName
            };

            if ( !string.IsNullOrWhiteSpace( giveParameters.PhoneNumber ) )
            {
                person.PhoneNumbers.Add( new PhoneNumber()
                {
                    Number = giveParameters.PhoneNumber,
                    NumberTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME ) ).Id
                } );
            }

            var rockContext = new RockContext();
            new PersonService( rockContext ).Add( person );

            // Need to save to get the person's Id
            rockContext.SaveChanges();

            if ( !person.Aliases.Any() )
            {
                person.Aliases.Add( new PersonAlias { AliasPersonId = person.Id, AliasPersonGuid = person.Guid } );
            }

            var familyGroupType = GroupTypeCache.GetFamilyGroupType();
            var adultId = familyGroupType.Roles.FirstOrDefault( r =>
                r.Guid == new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ).Id;

            var familyGroup = new Group
            {
                GroupTypeId = familyGroupType.Id,
                IsSecurityRole = false,
                IsSystem = false,
                IsActive = true,
                Name = person.LastName + " Family"
            };

            familyGroup.GroupLocations.Add( new GroupLocation()
            {
                GroupLocationTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME ) ).Id,
                Location = new Location()
                {
                    Street1 = giveParameters.Street1,
                    Street2 = giveParameters.Street2,
                    City = giveParameters.City,
                    State = giveParameters.State,
                    PostalCode = giveParameters.PostalCode,
                    Country = giveParameters.Country
                }
            } );

            new GroupService( rockContext ).Add( familyGroup );

            var groupMember = new GroupMember
            {
                IsSystem = false,
                GroupId = familyGroup.Id,
                PersonId = person.Id,
                GroupRoleId = adultId
            };

            familyGroup.Members.Add( groupMember );
            rockContext.SaveChanges();

            return person;
        }

        private HttpResponseMessage GenerateResponse( HttpStatusCode code, string message = null )
        {
            var response = new HttpResponseMessage( code );

            if ( !string.IsNullOrWhiteSpace( message ) )
            {
                response.Content = new StringContent( message );
            }

            return response;
        }
    }
}