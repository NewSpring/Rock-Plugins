using System;
using Rock.Model;

namespace cc.newspring.Apollos
{
    public class GiveParameters : PaymentParameters
    {
        public AmountDetail[] AmountDetails { get; set; }

        public int? SourceAccountId { get; set; }
    }

    public class AmountDetail
    {
        public int TargetAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}