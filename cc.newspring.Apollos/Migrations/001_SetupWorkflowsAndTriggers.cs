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

using Rock.Plugin;

namespace cc.newspring.Apollos.Migrations
{
    [MigrationNumber( 1, "1.2.0" )]
    public class SetupWorkflowsAndTriggers : Migration
    {
        private string categoryGuid = "65CCE790-BE3D-4C52-AB4C-0BA8FFEE630E";
        private string userDeleteTypeGuid = "C9D8F6A2-CE98-4DD5-8963-9403D02F9CC8";
        private string userSaveTypeGuid = "7CB0BE68-98B9-44FC-90E8-D78DD59DE3DC";
        private string userDeleteActivityGuid = "8DBFDDD9-38BF-4D4A-943D-0CEF90C7D6ED";
        private string userSaveActivityGuid = "4F94DD52-933F-441C-AC97-50C6612379A4";
        private string userDeleteActionGuid = "1AD61C3E-2315-4BC0-A35E-8271B48DA274";
        private string userSaveActionGuid = "98517B56-4089-46FE-839C-04C2128B4E93";
        private string entityTypeGuid = "6AD93C7E-E314-4618-8EC2-9A8DF9AEAE61";

        private string activeAttributeGuid = "65CB1840-9F36-4369-AC8E-7AB94BF18D1B";
        private string actionAttributeGuid = "B5B78DE9-41ED-4175-9DF5-E3E62ADEC388";
        private string syncAttributeGuid = "C166C5D7-FE59-45ED-B38A-5B7B08124CF2";
        private string tokenNameAttributeGuid = "3AF3C584-8687-495C-A474-8568AF5D44B4";
        private string tokenValueAttributeGuid = "8DF06A26-0544-436F-8034-BF86C465AD2B";
        private string orderAttributeGuid = "B3B900AB-27A0-4573-8144-0CEC65C0C381";

        private void DeleteByGuid( string guid, string table )
        {
            Sql( string.Format( "DELETE [{0}] WHERE [Guid] = '{1}'", table, guid ) );
        }

        private void DeleteWorkflowType( string guid )
        {
            DeleteByGuid( guid, "WorkflowType" );
        }

        private void DeleteWorkflowActivityType( string guid )
        {
            DeleteByGuid( guid, "WorkflowActivityType" );
        }

        private void DeleteWorkflowActionType( string guid )
        {
            DeleteByGuid( guid, "WorkflowActionType" );
        }

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "cc.newspring.Apollos.Workflow.Action.APISync", entityTypeGuid, false, true );

            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", activeAttributeGuid ); // Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Action", "Action", "The workflow that this action is under is triggered by what type of event", 0, @"", actionAttributeGuid ); // Action
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Sync URL", "SyncURL", "The specific URL endpoint this related entity type should synchronize with", 0, @"", syncAttributeGuid ); // Sync URL
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Name", "TokenName", "The key by which the token should be identified in the header of HTTP requests", 0, @"", tokenNameAttributeGuid ); // Token Name
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Value", "TokenValue", "The value of the token to authenticate with the URL endpoint", 0, @"", tokenValueAttributeGuid ); // Token Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( entityTypeGuid, "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", orderAttributeGuid ); // Order

            RockMigrationHelper.UpdateCategory( "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE", "Apollos API Sync Category", "fa fa-connectdevelop", "", categoryGuid, 0 );

            RockMigrationHelper.UpdateWorkflowType( false, true, "Apollos UserLogin Delete Workflow Type", "", categoryGuid, "Work", "fa fa-list-ol", 0, true, 0, userDeleteTypeGuid );
            RockMigrationHelper.UpdateWorkflowType( false, true, "Apollos UserLogin Save Workflow Type", "", categoryGuid, "Work", "fa fa-list-ol", 0, true, 0, userSaveTypeGuid );

            RockMigrationHelper.UpdateWorkflowActivityType( userDeleteTypeGuid, true, "Apollos UserLogin Delete Activity", "", true, 0, userDeleteActivityGuid );
            RockMigrationHelper.UpdateWorkflowActivityType( userSaveTypeGuid, true, "Apollos UserLogin Save Activity", "", true, 0, userSaveActivityGuid );

            RockMigrationHelper.UpdateWorkflowActionType( userDeleteActivityGuid, "Apollos UserLogin Delete Action", 0, entityTypeGuid, true, false, "", "", 1, "", userDeleteActionGuid );
            RockMigrationHelper.UpdateWorkflowActionType( userSaveActivityGuid, "Apollos UserLogin Save Action", 0, entityTypeGuid, true, false, "", "", 1, "", userSaveActionGuid );

            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, syncAttributeGuid, @"" ); // Sync URL
            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, tokenValueAttributeGuid, @"" ); // Token Value
            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, tokenNameAttributeGuid, @".apollos" ); // Token Name
            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, orderAttributeGuid, @"" ); // Order
            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, activeAttributeGuid, @"False" ); // Active
            RockMigrationHelper.AddActionTypeAttributeValue( userDeleteActionGuid, actionAttributeGuid, @"Delete" ); // Action

            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, syncAttributeGuid, @"" ); // Sync URL
            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, tokenValueAttributeGuid, @"" ); // Token Value
            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, tokenNameAttributeGuid, @".apollos" ); // Token Name
            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, orderAttributeGuid, @"" ); // Order
            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, activeAttributeGuid, @"False" ); // Active
            RockMigrationHelper.AddActionTypeAttributeValue( userSaveActionGuid, actionAttributeGuid, @"Save" ); // Action
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            // REMOVE all of AddActionTypeAttributeValue

            DeleteWorkflowActionType( userSaveActionGuid );
            DeleteWorkflowActionType( userDeleteActionGuid );

            DeleteWorkflowActivityType( userSaveActivityGuid );
            DeleteWorkflowActivityType( userDeleteActivityGuid );

            DeleteWorkflowType( userSaveTypeGuid );
            DeleteWorkflowType( userDeleteTypeGuid );

            RockMigrationHelper.DeleteCategory( categoryGuid );

            // DELETE ATTRIBUTES HERE

            RockMigrationHelper.DeleteEntityType( entityTypeGuid );
        }
    }
}