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
        protected string MinistryMetricGuid = "A5F29054-EC70-4BA3-B181-E2A62D11A929";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddDefinedType( "Metric", "Ministry Metrics", "", MinistryMetricGuid, @"" );
            RockMigrationHelper.AddDefinedTypeAttribute( "A5F29054-EC70-4BA3-B181-E2A62D11A929", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "SQL Statement", "SQLStatement", "", 1007, "", "5EEB8E34-B0B9-43C0-9124-984728D36C98" );

            RockMigrationHelper.AddDefinedValue( "A5F29054-EC70-4BA3-B181-E2A62D11A929", "KidSpring Sunday", "", "1ECE28F7-63BE-4495-A4AB-6EC98A10D48E", false );
            RockMigrationHelper.AddDefinedValue( "A5F29054-EC70-4BA3-B181-E2A62D11A929", "Total numbers for Fuse", "", "4BFBEEC7-48FE-4FE6-9DB8-7302628DC6A4", false );
            RockMigrationHelper.AddDefinedValue( "A5F29054-EC70-4BA3-B181-E2A62D11A929", "Total unique Sunday volunteers for KidSpring", "", "C565CC8A-DB9D-4B1F-BD06-2930EE9DD79E", false );
            RockMigrationHelper.AddDefinedValue( "A5F29054-EC70-4BA3-B181-E2A62D11A929", "Total unique weekly volunteers for KidSpring", "", "265BEEC8-1F4D-4543-AB82-B61E1EA96D19", false );

            RockMigrationHelper.AddDefinedValueAttributeValue( "1ECE28F7-63BE-4495-A4AB-6EC98A10D48E", "5EEB8E34-B0B9-43C0-9124-984728D36C98", @"
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
			                -- KidSpring grouptypes
			                GroupTypeId >= 720
			                AND GroupTypeId <= 725
	                ) GroupIds
	                ON GroupId = GroupIds.Id
		                -- Greenwood campus
		                AND CampusId = 8
                ) GWDAttendance
	                ON Attendance.Id = GWDAttendance.Id
                WHERE DidAttend = 1
	                -- Sunday attendance only
	                AND DATEPART(DW, StartDateTime) = 1	
	                -- Within the last week
	                AND StartDateTime >= @LastSunday
	                AND StartDateTime < @Today" );

            RockMigrationHelper.AddDefinedValueAttributeValue( "265BEEC8-1F4D-4543-AB82-B61E1EA96D19", "5EEB8E34-B0B9-43C0-9124-984728D36C98", @"
                DECLARE @Today datetime = ''2015-03-30''
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
			                -- KidSpring grouptypes
			                GroupTypeId >= 720
			                AND GroupTypeId <= 725
	                ) GroupIds
	                ON GroupId = GroupIds.Id
		                -- Greenwood campus
		                AND CampusId = 8
                ) GWDAttendance
	                ON Attendance.Id = GWDAttendance.Id
                WHERE DidAttend = 1	
	                -- Within the last week
	                AND StartDateTime >= @LastSunday
	                AND StartDateTime < @Today" );

            RockMigrationHelper.AddDefinedValueAttributeValue( "4BFBEEC7-48FE-4FE6-9DB8-7302628DC6A4", "5EEB8E34-B0B9-43C0-9124-984728D36C98", @"
                --DECLARE @Today datetime = ''2015-03-26''
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
			                -- Fuse attendee
			                GroupTypeId = 710
	                ) GroupIds
	                ON GroupId = GroupIds.Id
		                -- Boiling Springs campus
		                AND CampusId = 3
                ) BSPAttendance
	                ON Attendance.Id = BSPAttendance.Id
                WHERE DidAttend = 1	
	                -- Within the last week
	                AND StartDateTime > DATEADD(day, -7, @Today)
	                AND StartDateTime < @Today" );

            RockMigrationHelper.AddDefinedValueAttributeValue( "C565CC8A-DB9D-4B1F-BD06-2930EE9DD79E", "5EEB8E34-B0B9-43C0-9124-984728D36C98", @"
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
                – KidSpring grouptypes
                GroupTypeId >= 720
                AND GroupTypeId <= 725
                ) GroupIds
                ON GroupId = GroupIds.Id
                – Greenwood campus
                AND CampusId = 8
                ) GWDAttendance
                ON Attendance.Id = GWDAttendance.Id
                WHERE DidAttend = 1
                – Sunday attendance only
                AND DATEPART(DW, StartDateTime) = 1	
                – Within the last week
                AND StartDateTime >= @LastSunday
                AND StartDateTime < @Today" );

        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            /// ************************************************************************
            /// 
            /// Once the parent defined type is deleted the children are deleted as well
            /// 
            /// ************************************************************************
            RockMigrationHelper.DeleteDefinedType( MinistryMetricGuid ); // Ministry Metrics
            RockMigrationHelper.DeleteAttribute( "5EEB8E34-B0B9-43C0-9124-984728D36C98" ); // SQL Statement Attribute
        }
    }
}