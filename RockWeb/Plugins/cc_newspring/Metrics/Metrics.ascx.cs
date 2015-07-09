// <copyright>
// Copyright 2015 by NewSpring Church
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using RestSharp;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Reporting.Dashboard;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.cc_newspring.Metrics
{
    /// <summary>
    /// All Church Metrics Block
    /// </summary>
    [DisplayName( "All Church Metrics Block" )]
    [Category( "Metrics" )]
    [Description( "All Church Metrics Block" )]
    [TextField( "Number of Columns", "", false, DefaultValue = "3", Order = 1 )]
    [CustomDropdownListField( "Metric Display Type", "", "Text,Line,Donut", Order = 2 )]
    [CustomDropdownListField( "Metric Period", "", "This Week,Last Week,One Year Ago,YTD", DefaultValue = "YTD", Order = 3 )]
    [MetricCategoriesField( "Metric Source", "Select the metric to include in this chart.", false, "", "", 4 )]
    // [SlidingDateRangeField( "Date Range", Key = "SlidingDateRange", DefaultValue = "1||4||", Order = 7 )]
    // [LinkedPage( "Detail Page", "Select the page to navigate to when the chart is clicked", Order = 8 )]
    public partial class Metrics : Rock.Web.UI.RockBlock
    {
        #region Control Methods

        // <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnInit( e );

            // Output variables direct to the ascx
            metricTitle.Value = BlockName;
            metricDisplay.Value = GetAttributeValue( "MetricDisplayType" );
            metricWidth.Value = GetAttributeValue( "NumberofColumns" );

            var churchMetricSource = GetMetricIds( "MetricSource" );
            var churchMetricPeriod = GetAttributeValue( "MetricPeriod" );

            // Show the warning if no data is selected
            churchMetricWarning.Visible = !churchMetricSource.Any();

            var newMetric = new MetricService( new RockContext() ).GetByIds( churchMetricSource );

            if ( GetAttributeValue( "MetricDisplayType" ) == "Text" )
            {
                foreach ( var metric in newMetric )
                {
                    var churchMetricValue = metric.MetricValues;
                    var sortedMetric = churchMetricValue
                        .OrderByDescending( c => c.MetricValueDateTime )
                        .ToArray();

                    if ( churchMetricPeriod == "This Week" )
                    {
                        var currentWeek = sortedMetric[0].YValue;
                        var lastWeek = sortedMetric[1].YValue;

                        if ( currentWeek > lastWeek )
                        {
                            metricClass.Value = "fa-caret-up brand-success";
                            metricNumber.Value = string.Format( "{0:n0}", currentWeek );
                        }
                        else
                        {
                            metricClass.Value = "fa-caret-down brand-danger";
                            metricNumber.Value = string.Format( "{0:n0}", currentWeek );
                        }
                    }
                    else if ( churchMetricPeriod == "Last Week" )
                    {
                        var lastWeek = sortedMetric[1].YValue;

                        metricNumber.Value = string.Format( "{0:n0}", lastWeek );
                    }
                    else if ( churchMetricPeriod == "One Year Ago" )
                    {
                        // Search the DB
                        var lastYear = churchMetricValue
                            .Where( a => a.MetricValueDateTime > DateTime.Now.AddYears( -1 ).AddDays( -7 ) && a.MetricValueDateTime < DateTime.Now.AddYears( -1 ).AddDays( 1 ) )
                            .Select( a => a.YValue )
                            .ToArray();

                        // Output the value
                        metricNumber.Value = string.Format( "{0:n0}", lastYear[0] );
                    }
                    else
                    {
                        var ytdSince = new DateTime( DateTime.Now.Year, 1, 1 );

                        var metricSum = churchMetricValue.Where( a => a.MetricValueDateTime > ytdSince ).Select( a => a.YValue ).ToArray();
                        metricNumber.Value = string.Format( "{0:n0}", metricSum.Sum() );
                    }
                }
            }
            else if ( GetAttributeValue( "MetricDisplayType" ) == "Line" )
            {
                foreach ( var metric in newMetric )
                {
                    var churchMetricValue = metric.MetricValues;

                    currentYear.Value = DateTime.Now.Year.ToString();
                    previousYear.Value = DateTime.Now.AddYears( -1 ).Year.ToString();

                    // Create an array of of labels
                    var metricLabelsArray = churchMetricValue.Where( a => a.MetricValueDateTime > DateTime.Now.AddDays( -42 ) )
                        .OrderBy( a => a.MetricValueDateTime )
                        .Select( a => new DateTime( a.MetricValueDateTime.Value.Year, a.MetricValueDateTime.Value.Month, a.MetricValueDateTime.Value.Day ).ToString( "MMMM dd" ) )
                        .ToArray();

                    var metricLabelsString = string.Join( ",", metricLabelsArray );

                    // Format the array of labels for output
                    metricLabels.Value = "'" + metricLabelsString.Replace( ",", "','" ) + "'";

                    // Create an array of data points (Current Year)
                    //var metricDataPointSumsCurrentYear = churchMetricValue
                    //    .Where( a => a.MetricValueDateTime > DateTime.Now.AddMonths( -6 ) )
                    //    .Select( a => new { Month = a.MetricValueDateTime.Value.Month, YValue = a.YValue } )
                    //    .GroupBy( a => a.Month )
                    //    .Select( a => new { a.Key, Sum = a.Sum( v => v.YValue ).ToString() } )
                    //    .OrderBy( a => a.Key )
                    //    .Select( a => a.Sum)
                    //    .ToArray();

                    var metricDataPointSumsCurrentYear = churchMetricValue
                        .Where( a => a.MetricValueDateTime > DateTime.Now.AddDays( -42 ) )
                        .OrderBy( a => a.MetricValueDateTime )
                        .Select( a => string.Format( "{0:0}", a.YValue ) )
                        .ToArray();

                    var metricDataPointStringCurrent = string.Join( ",", metricDataPointSumsCurrentYear );

                    // Format the array of sums for output
                    metricDataPointsCurrent.Value = "'" + metricDataPointStringCurrent.Replace( ",", "','" ) + "'";

                    // Create an array of data points (Current Year)
                    var metricDataPointSumsPreviousYear = churchMetricValue
                        .Where( a => a.MetricValueDateTime > DateTime.Now.AddDays( -1 ).AddMonths( -42 ) && a.MetricValueDateTime < DateTime.Now.AddYears( -1 ) )
                        .OrderBy( a => a.MetricValueDateTime )
                        .Select( a => string.Format( "{0:0}", a.YValue ) )
                        .ToArray();

                    var metricDataPointStringPrevious = string.Join( ",", metricDataPointSumsPreviousYear );

                    // Format the array of sums for output
                    metricDataPointsPrevious.Value = "'" + metricDataPointStringPrevious.Replace( ",", "','" ) + "'";
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the metrics.
        /// </summary>
        /// <returns></returns>
        ///
        /// <value>
        /// The metrics.
        /// </value>
        public List<int> GetMetricIds( string metricAttribute )
        {
            var metricCategories = Rock.Attribute.MetricCategoriesFieldAttribute.GetValueAsGuidPairs( GetAttributeValue( metricAttribute ) );

            var metricGuids = metricCategories.Select( a => a.MetricGuid ).ToList();
            return new MetricService( new Rock.Data.RockContext() ).GetByGuids( metricGuids ).Select( a => a.Id ).ToList();
        }

        #endregion
    }
}