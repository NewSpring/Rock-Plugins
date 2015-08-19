using System;
using Rock.Model;

namespace cc.newspring.Apollos
{
    public class ScheduleParameters : GiveParameters
    {
        public DateTime? StartDate { get; set; }

        public Guid? FrequencyValueGuid { get; set; }
    }
}