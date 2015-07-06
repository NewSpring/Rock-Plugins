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
    [CustomDropdownListField( "Metric Display Type", "", "Text,Line,Donut" )]
    [CustomDropdownListField( "Metric Period", "", "This Week,YTD" )]
    [MetricCategoriesField( "Metric Source", "Select the metric to include in this chart.", false, "", "", 4 )]
    [SlidingDateRangeField( "Date Range", Key = "SlidingDateRange", DefaultValue = "1||4||", Order = 7 )]
    [LinkedPage( "Detail Page", "Select the page to navigate to when the chart is clicked", Order = 8 )]
    public partial class Metrics : RockBlock
    {
        #region Control Methods

        // <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnInit( e );

            metricTitle.Value = BlockName;

            var churchMetricSource = GetMetricIds( "MetricSource" );
            metricDisplay.Value = GetAttributeValue( "MetricDisplayType" );
            var churchMetricPeriod = GetAttributeValue( "MetricPeriod" );

            churchMetricWarning.Visible = !churchMetricSource.Any();

            var newMetric = new MetricService( new RockContext() ).GetByIds( churchMetricSource );

            foreach ( var metric in newMetric )
            {
                var churchMetricValue = metric.MetricValues;

                if ( churchMetricPeriod == "YTD" )
                {
                    // Here's an the numbers, make sure that these metrics are after jan 1 of the current year

                    // Use this to get entries since this date
                    var ytdSince = new DateTime( DateTime.Now.Year, 1, 1 );

                    var metricSum = churchMetricValue.Where( a => a.MetricValueDateTime > ytdSince ).Select( a => a.YValue ).ToArray();
                    metricNumber.Value = string.Format( "{0:n0}", metricSum.Sum() );
                }
                else
                {
                    var sortedMetric = churchMetricValue.OrderByDescending( c => c.CreatedDateTime ).ToArray();

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