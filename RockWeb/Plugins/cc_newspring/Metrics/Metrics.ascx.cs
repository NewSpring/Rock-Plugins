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
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            this.BlockUpdated += Block_BlockUpdated;

            var churchMetricSource = GetMetricIds( "MetricSource" );

            var newMetric = new MetricService( new RockContext() ).GetByIds( churchMetricSource );

            foreach ( var metric in newMetric )
            {
                var churchMetricValue = metric.MetricValues;

                var sortedMetric = churchMetricValue.OrderByDescending( c => c.CreatedDateTime ).ToArray();

                var currentWeek = sortedMetric[0].YValue;
                var lastWeek = sortedMetric[1].YValue;

                if ( currentWeek > lastWeek )
                {
                    metricType.Value = "greater";
                }
                else
                {
                    metricType.Value = "less";
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            var metricId = GetMetricIds( "MetricSource" );

            string pieChartURL = string.Format( "~/api/MetricValues/GetSummary?metricIdList={0}", metricId.AsDelimited( "," ) );

            pieAttendance.DataSourceUrl = pieChartURL;

            LoadChart();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the Block control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            LoadChart();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the chart.
        /// </summary>
        public void LoadChart()
        {
            //pieTitle.Text = this.Title;
            //pieSubTitle.Text = this.Subtitle;
            pieAttendance.ShowTooltip = true;
            pieAttendance.Options.SetChartStyle( this.GetAttributeValue( "ChartStyle" ).AsGuidOrNull() );

            lineAttendance.ShowTooltip = true;
            lineAttendance.Options.SetChartStyle( this.GetAttributeValue( "ChartStyle" ).AsGuidOrNull() );

            var pieChartMetrics = GetMetricIds( "PieChartMetric" );
            var lineChartMetrics = GetMetricIds( "LineChartMetric" );

            string pieChartURL = string.Format( "~/api/MetricValues/GetSummary?metricIdList={0}", pieChartMetrics.AsDelimited( "," ) );
            string lineChartURL = string.Format( "~/api/MetricValues/GetSummary?metricIdList={0}", lineChartMetrics.AsDelimited( "," ) );

            var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( this.GetAttributeValue( "SlidingDateRange" ) ?? string.Empty );
            if ( dateRange != null )
            {
                if ( dateRange.Start.HasValue )
                {
                    pieChartURL += string.Format( "&startDate={0}", dateRange.Start.Value.ToString( "o" ) );
                    lineChartURL += string.Format( "&startDate={0}", dateRange.Start.Value.ToString( "o" ) );
                }

                if ( dateRange.End.HasValue )
                {
                    pieChartURL += string.Format( "&endDate={0}", dateRange.End.Value.ToString( "o" ) );
                    lineChartURL += string.Format( "&endDate={0}", dateRange.End.Value.ToString( "o" ) );
                }
            }

            var metricValueType = this.GetAttributeValue( "MetricValueTypes" ).ConvertToEnumOrNull<MetricValueType>() ?? Rock.Model.MetricValueType.Measure;

            pieChartURL += string.Format( "&metricValueType={0}", metricValueType );
            lineChartURL += string.Format( "&metricValueType={0}", metricValueType );

            string[] entityValues = ( GetAttributeValue( "Entity" ) ?? string.Empty ).Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
            EntityTypeCache entityType = null;
            if ( entityValues.Length >= 1 )
            {
                entityType = EntityTypeCache.Read( entityValues[0].AsGuid() );
            }

            if ( entityValues.Length == 2 )
            {
                // entity id specified by block setting
                if ( entityType != null )
                {
                    pieChartURL += string.Format( "&entityTypeId={0}", entityType.Id );
                    lineChartURL += string.Format( "&entityTypeId={0}", entityType.Id );
                    int? entityId = entityValues[1].AsIntegerOrNull();
                    if ( entityId.HasValue )
                    {
                        pieChartURL += string.Format( "&entityId={0}", entityId );
                        lineChartURL += string.Format( "&entityId={0}", entityId );
                    }
                }
            }
            else
            {
                // entity id comes from context
                Rock.Data.IEntity contextEntity;
                if ( entityType != null )
                {
                    contextEntity = this.ContextEntity( entityType.Name );
                }
                else
                {
                    contextEntity = this.ContextEntity();
                }

                if ( contextEntity != null )
                {
                    pieChartURL += string.Format( "&entityTypeId={0}&entityId={1}", EntityTypeCache.GetId( contextEntity.GetType() ), contextEntity.Id );
                    lineChartURL += string.Format( "&entityTypeId={0}&entityId={1}", EntityTypeCache.GetId( contextEntity.GetType() ), contextEntity.Id );
                }
            }

            pieAttendance.DataSourceUrl = pieChartURL;
            lineAttendance.DataSourceUrl = lineChartURL;

            //// pieAttendance.PieOptions.tilt = 0.5;
            //// pieAttendance.ChartHeight =

            pieAttendance.PieOptions.label = new PieLabel { show = true };
            pieAttendance.PieOptions.label.formatter = @"
                    function labelFormatter(label, series) {
                    	return ""<div style='font-size:8pt; text-align:center; padding:2px; '>"" + label + ""<br/>"" + Math.round(series.percent) + ""%</div>"";
                    }
                    ".Trim();
            pieAttendance.Legend.show = false;

            lineAttendance.MetricId = lineChartMetrics.FirstOrDefault();

            lineAttendance.MetricValueType = metricValueType;
            lineAttendance.StartDate = dateRange.Start;
            lineAttendance.EndDate = dateRange.End;
            lineAttendance.CombineValues = false;

            nbMetricWarning.Visible = !pieChartMetrics.Any();
            lineMetricWarning.Visible = !lineChartMetrics.Any();
        }

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