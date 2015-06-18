////////////////////////////////
// Live Video All Staff Plugin
//
// Author
// -------
// Edolyne Long
// NewSpring Church
//
// Version 0.9
////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using DDay.iCal;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.cc_newspring.AllStaffLive
{
    [DisplayName( "All Staff Live" )]
    [Category( "Video" )]
    [Description( "Live video all staff display" )]
    [TextField( "Ooyala Content ID", "Paste the Ooyala Content ID For All Staff Live Here" )]
    [SchedulesField( "Live Schedule", "Choose a schedule for all staff to be live" )]
    public partial class AllStaffLive : Rock.Web.UI.RockBlock
    {
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // Set Times To Check Against
            var serverDate = DateTime.Now.Date;
            string serverDay = DateTime.Now.DayOfWeek.ToString();
            var serverTime = DateTime.Now.TimeOfDay;

            var scheduleGuids = GetAttributeValue( "LiveSchedule" ).ToString();
            var scheduleArray = scheduleGuids.Split( ',' ).AsGuidList().ToArray();

            var scheduleService = new ScheduleService( new RockContext() );

            // Support for multiples schedules loops through each
            foreach ( var scheduleGuid in scheduleArray )
            {
                var schedule = scheduleService.Get( scheduleGuid );

                var calendarList = iCalendar.LoadFromStream( new StringReader( schedule.iCalendarContent ) );

                // Get the friendly schedule text
                scheduleText.Value = schedule.FriendlyScheduleText.ToString();

                // Get the start & end dates for the schedule

                // Get the start date of the schedule
                var scheduleStartDate = new DateTime( calendarList[0].Events[0].DTEnd.Year, calendarList[0].Events[0].DTEnd.Month, calendarList[0].Events[0].DTEnd.Day );

                // Get the schedule start time
                var scheduleStartTime = new TimeSpan( schedule.StartTimeOfDay.Hours, schedule.StartTimeOfDay.Minutes, schedule.StartTimeOfDay.Seconds );

                // Make the offset open minutes negative
                var offsetMinutes = schedule.CheckInStartOffsetMinutes * -1 ?? 0;

                // Create the new schedule start time
                var scheduleOpenTime = scheduleStartTime.Add( new TimeSpan( 0, offsetMinutes, 0 ) );

                if ( calendarList.Any() )
                {
                    DateTime? scheduleExpiration;
                    bool hasRecurringRules = false;

                    var calendarInstance = calendarList[0].Events[0];

                    // Check for recurring and set the appropriate expriation option
                    if ( calendarInstance.RecurrenceRules[0] != null )
                    {
                        scheduleExpiration = calendarInstance.RecurrenceRules[0].Until;
                        hasRecurringRules = true;
                    }
                    else
                    {
                        scheduleExpiration = calendarInstance.DTEnd.Date;
                    }

                    // Check to make sure that the schedule isn't expired
                    if ( scheduleExpiration >= serverDate )
                    {

                        // Get the end time
                        var scheduleEndTime = calendarInstance.DTEnd.TimeOfDay;

                        // Get the day
                        string scheduleDay = calendarInstance.DTEnd.DayOfWeek.ToString();


                        if ( hasRecurringRules )
                        {
                            var liveDays = calendarInstance.RecurrenceRules[0].ByDay;

                            var recurringDays = liveDays.Select( d => d.DayOfWeek.ToString() ).ToList();

                            foreach ( var recurringDay in recurringDays )
                            {
                                if ( serverDay.Equals( recurringDay ) )
                                {
                                    if ( serverTime >= scheduleOpenTime && serverTime <= scheduleEndTime )
                                    {
                                        liveFeedStatus.Value = "on";
                                    }
                                    else
                                    {
                                        liveFeedStatus.Value = "off";
                                    }
                                }
                                else
                                {
                                    liveFeedStatus.Value = "off";
                                }
                            }
                        }
                        else
                        {
                            if ( serverTime >= scheduleOpenTime && serverTime <= scheduleEndTime )
                            {
                                liveFeedStatus.Value = "on";
                            }
                            else
                            {
                                liveFeedStatus.Value = "off";
                            }
                        }

                    }
                    else
                    {
                        // Schedule is expired, pass an expired value for future expansion
                        liveFeedStatus.Value = "expired";
                    }
                }

                if ( liveFeedStatus.Value.Equals( "on" ) )
                {
                    break;
                }
            }

            // Set the ooyala id
            ooyalaId.Value = GetAttributeValue( "OoyalaContentID" );
        }
    }
}