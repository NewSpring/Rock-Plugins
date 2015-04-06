using System;
using Rock.Model;

namespace cc.newspring.Apollos
{
    public class GiveParameters
    {
        public AccountAmount[] Amounts { get; set; }

        public string Email { get; set; }

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string RoutingNumber { get; set; }

        public string CCV { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int? PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class AccountAmount
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}