// <copyright>
// Copyright 2013 by the Spark Development Network
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rock;
using Rock.Data;
using Rock.Plugin;

/// <summary>
/// Adds the queries needed for the Ministry Metric block
/// </summary>
namespace cc.newspring.Metrics.Migrations
{
    [MigrationNumber( 1, "1.3.4" )]
    public class AddSQLTypes : Migration
    {
        // Defined guid variables
        protected string MinistryMetricGuid = "A5F29054-EC70-4BA3-B181-E2A62D11A929";

        protected string SQLAttributeGuid = "5EEB8E34-B0B9-43C0-9124-984728D36C98";

        protected string EditorFieldTypeGuid = "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5";

        protected string CurrentDefinedValueGuid = string.Empty;

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddDefinedType( "Metric", "Ministry Metrics", "", MinistryMetricGuid, @"" );
            RockMigrationHelper.AddDefinedTypeAttribute( MinistryMetricGuid, EditorFieldTypeGuid, "SQL Statement", "SQLStatement", "", 1007, "", SQLAttributeGuid );

            // KidSpring Sunday Attendance
            CurrentDefinedValueGuid = "1ECE28F7-63BE-4495-A4AB-6EC98A10D48E";
            RockMigrationHelper.AddDefinedValue( MinistryMetricGuid, "KidSpring Sunday Attendance", "", CurrentDefinedValueGuid, false );
            RockMigrationHelper.AddDefinedValueAttributeValue( CurrentDefinedValueGuid, SQLAttributeGuid, @"
                DECLARE @Today datetime = GETDATE()
                DECLARE @LastSunday datetime = DATEADD(
	                DAY, -((DATEPART(DW, @Today) + 6) % 7), @Today
                )

                SELECT COUNT(1)
                FROM Attendance
                INNER JOIN
                (
	                SELECT Attendance.Id
	                FROM Attendance
	                INNER JOIN (
		                SELECT ID FROM [Group]
		                WHERE
			                –- Filter by Grouptype
			                GroupTypeId >= 720
			                AND GroupTypeId <= 725
	                ) GroupIds
                    –- Filter by Group
	                ON GroupId = GroupIds.Id
		                –- Filter by Campus
		                AND CampusId = {{campusId}}
                ) CampusAttendance
	                ON Attendance.Id = CampusAttendance.Id
                WHERE DidAttend = 1
	                -- Filter by Sundays only
	                AND DATEPART(DW, StartDateTime) = 1
	                -- Filter by Date Range
	                AND StartDateTime >= @LastSunday
	                AND StartDateTime < @Today
            " );

            // Total unique weekly volunteers for KidSpring
            CurrentDefinedValueGuid = "265BEEC8-1F4D-4543-AB82-B61E1EA96D19";
            RockMigrationHelper.AddDefinedValue( MinistryMetricGuid, "Total unique weekly volunteers for KidSpring", "", CurrentDefinedValueGuid, false );
            RockMigrationHelper.AddDefinedValueAttributeValue( CurrentDefinedValueGuid, SQLAttributeGuid, @"
                DECLARE @Today datetime = GETDATE()
                DECLARE @LastSunday datetime = DATEADD(
	                DAY, -((DATEPART(DW, @Today) + 6) % 7), @Today
                )

                SELECT COUNT(DISTINCT PersonAliasId)
                FROM Attendance
                INNER JOIN
                (
	                SELECT Attendance.Id
	                FROM Attendance
	                INNER JOIN (
		                SELECT ID FROM [Group]
		                WHERE
			                –- Filter by Grouptype
			                GroupTypeId >= 720
			                AND GroupTypeId <= 725
	                ) GroupIds
                    –- Filter by Group
	                ON GroupId = GroupIds.Id
		                –- Filter by Campus
		                AND CampusId = {{campusId}}
                ) CampusAttendance
	                ON Attendance.Id = CampusAttendance.Id
                WHERE DidAttend = 1
	                –- Filter by Date Range
	                AND StartDateTime >= @LastSunday
	                AND StartDateTime < @Today
            " );

            // Total Fuse Weekly Attendance
            CurrentDefinedValueGuid = "4BFBEEC7-48FE-4FE6-9DB8-7302628DC6A4";
            RockMigrationHelper.AddDefinedValue( MinistryMetricGuid, "Total weekly attendance for Fuse", "", CurrentDefinedValueGuid, false );
            RockMigrationHelper.AddDefinedValueAttributeValue( CurrentDefinedValueGuid, SQLAttributeGuid, @"
                DECLARE @Today datetime = GETDATE()

                SELECT COUNT(DISTINCT PersonAliasId)
                FROM Attendance
                INNER JOIN
                (
	                SELECT Attendance.Id
	                FROM Attendance
	                INNER JOIN (
		                SELECT ID FROM [Group]
		                WHERE
			                –- Filter by Grouptype
			                GroupTypeId = 710
	                ) GroupIds
                    –- Filter by Group
	                ON GroupId = GroupIds.Id
		                –- Filter by Campus
		                AND CampusId = {{campusId}}
                ) CampusAttendance
	                ON Attendance.Id = CampusAttendance.Id
                WHERE DidAttend = 1
	                –- Filter by Date Range
	                AND StartDateTime > DATEADD(day, -7, @Today)
	                AND StartDateTime < @Today
            " );

            // Total unique Sunday volunteers for KidSpring
            CurrentDefinedValueGuid = "C565CC8A-DB9D-4B1F-BD06-2930EE9DD79E";
            RockMigrationHelper.AddDefinedValue( MinistryMetricGuid, "Total unique Sunday volunteers for KidSpring", "", CurrentDefinedValueGuid, false );
            RockMigrationHelper.AddDefinedValueAttributeValue( CurrentDefinedValueGuid, SQLAttributeGuid, @"
                DECLARE @Today datetime = GETDATE()
                DECLARE @LastSunday datetime = DATEADD(
                    DAY, -((DATEPART(DW, @Today) + 6) % 7), @Today
                )
                SELECT COUNT(DISTINCT PersonAliasId)
                FROM Attendance
                INNER JOIN
                (
                    SELECT Attendance.Id
                    FROM Attendance
                    INNER JOIN (
                        SELECT ID FROM [Group]
                        WHERE
                        –- Filter by Grouptype
                        GroupTypeId >= 720
                        AND GroupTypeId <= 725
                    ) GroupIds
                    -- Filter by Group
                    ON GroupId = GroupIds.Id
                    -- Filter by Campus
                    AND CampusId = {{campusId}}
                ) CampusAttendance
                    ON Attendance.Id = CampusAttendance.Id
                WHERE DidAttend = 1
                    –- Filter Sundays only
                    AND DATEPART(DW, StartDateTime) = 1
                    –- Filter by Date Range
                    AND StartDateTime >= @LastSunday
                    AND StartDateTime < @Today
            " );

            // Unique Volunteers Attended
            CurrentDefinedValueGuid = "1562B10F-6624-474D-B347-ADDB25ED39E3";
            RockMigrationHelper.AddDefinedValue( MinistryMetricGuid, "Unique Volunteers Attended", "", CurrentDefinedValueGuid, false );
            RockMigrationHelper.AddDefinedValueAttributeValue( CurrentDefinedValueGuid, SQLAttributeGuid, @"
                DECLARE @Today datetime = GETDATE()

                SELECT COUNT(DISTINCT PersonAliasId)
                FROM Attendance
                INNER JOIN
                (
	                SELECT Attendance.Id
	                FROM Attendance
	                -- Filter By Group
	                WHERE GroupId = 206016
	                -- Filter by Campus
	                AND CampusId = {{campusId}}
                ) CampusAttendance
	                ON Attendance.Id = CampusAttendance.Id
                WHERE DidAttend = 1
	                -- Filter by Date Range
	                AND StartDateTime > DATEADD(week, -6, @Today)
	                AND StartDateTime < @Today
            " );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            /// ****************************************************************************
            ///
            /// Once the parent defined type is deleted the child values are deleted as well
            ///
            /// ****************************************************************************
            RockMigrationHelper.DeleteDefinedType( MinistryMetricGuid ); // Ministry Metrics
            RockMigrationHelper.DeleteAttribute( SQLAttributeGuid ); // SQL Statement Attribute
        }
    }
}