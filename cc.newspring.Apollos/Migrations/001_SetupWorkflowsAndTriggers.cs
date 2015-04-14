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
using Rock.Model;
using Rock.Plugin;

namespace cc.newspring.Apollos.Migrations
{
    [MigrationNumber( 1, "1.2.0" )]
    public class SetupWorkflowsAndTriggers : Migration
    {
        private string categoryGuid = "65CCE790-BE3D-4C52-AB4C-0BA8FFEE630E";
        private string apiSyncGuid = "6AD93C7E-E314-4618-8EC2-9A8DF9AEAE61";
        private string apollosAuthGuid = "47CD1C76-3DAE-4453-8E4A-129BB2AE7348";

        private string userDeleteTypeGuid = "C9D8F6A2-CE98-4DD5-8963-9403D02F9CC8";
        private string userSaveTypeGuid = "7CB0BE68-98B9-44FC-90E8-D78DD59DE3DC";
        private string userDeleteActivityGuid = "8DBFDDD9-38BF-4D4A-943D-0CEF90C7D6ED";
        private string userSaveActivityGuid = "4F94DD52-933F-441C-AC97-50C6612379A4";
        private string userDeleteActionGuid = "1AD61C3E-2315-4BC0-A35E-8271B48DA274";
        private string userSaveActionGuid = "98517B56-4089-46FE-839C-04C2128B4E93";

        private string personDeleteTypeGuid = "8F066EB1-95E9-43C7-81F7-D6A7CB27B90B";
        private string personSaveTypeGuid = "C1EC88CB-7F52-4938-ADEF-C28959F38F96";
        private string personDeleteActivityGuid = "5A88E7AC-4022-4677-83F7-C170E904D2E0";
        private string personSaveActivityGuid = "CB571DBE-3CA0-425B-B63D-89C8380177C7";
        private string personDeleteActionGuid = "615A7E78-331E-4329-8A84-F5AF1B8A03A8";
        private string personSaveActionGuid = "0D26EC13-8B9F-4732-ADAA-8CEABE6EC9AC";

        private string activeAttributeGuid = "65CB1840-9F36-4369-AC8E-7AB94BF18D1B";
        private string actionAttributeGuid = "B5B78DE9-41ED-4175-9DF5-E3E62ADEC388";
        private string syncAttributeGuid = "C166C5D7-FE59-45ED-B38A-5B7B08124CF2";
        private string tokenNameAttributeGuid = "3AF3C584-8687-495C-A474-8568AF5D44B4";
        private string tokenValueAttributeGuid = "8DF06A26-0544-436F-8034-BF86C465AD2B";
        private string orderAttributeGuid = "B3B900AB-27A0-4573-8144-0CEC65C0C381";

        private void DeleteTriggersByCategory( string guid )
        {
            Sql( string.Format( @"
                DELETE
                FROM WorkflowTrigger t
                WHERE t.WorkflowTypeId IN (
	                SELECT w.Id
	                FROM WorkflowType w
	                JOIN Category c ON c.Id = w.CategoryId
	                WHERE c.Guid = '{0}'
                )", guid ) );
        }

        private void CreateSingleTrigger( string modelName, WorkflowTriggerType triggerType, string qualifierColumn, string qualifierValue, string workflowTypeGuid, string description )
        {
            var insertQuery = string.Format( @"
                INSERT INTO [dbo].[WorkflowTrigger]
                   ([IsSystem]
                   ,[EntityTypeId]
                   ,[EntityTypeQualifierColumn]
                   ,[EntityTypeQualifierValue]
                   ,[WorkflowTypeId]
                   ,[WorkflowTriggerType]
                   ,[WorkflowName]
                   ,[Guid]
                   ,[IsActive])
                VALUES
                   (0
                   ,(SELECT Id FROM EntityType WHERE NAME = 'Rock.Model.{0}')
                   ,'{1}'
                   ,{2}
                   ,(SELECT Id FROM WorkflowType WHERE Guid = '{3}')
                   ,{4}
                   ,'Apollos {5} Trigger'
                   ,NEWID()
                   ,1)", modelName, qualifierColumn, qualifierValue, workflowTypeGuid, (int)triggerType, description );

            Sql( insertQuery );
        }

        private void DeleteAttributesByEntity( string guid )
        {
            Sql( string.Format( @"
                SELECT a.*
                FROM Attribute a
                WHERE a.EntityTypeId IN (
	                SELECT w.Id
	                FROM EntityType w
	                WHERE w.Guid = '{0}'
                )", guid ) );
        }

        private void DeleteAttributeValuesByAction( string guid )
        {
            Sql( string.Format( @"
                DELETE
                FROM AttributeValue v
                WHERE v.EntityId IN (
	                SELECT t.Id
	                FROM WorkflowActionType t
	                WHERE t.Guid = '{0}'
                )", guid ) );
        }

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

        private void SetupAttributeValues( string actionGuid, string actionName, string entityName )
        {
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, syncAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenValueAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenNameAttributeGuid, @".apollos" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, orderAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, activeAttributeGuid, @"False" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, actionAttributeGuid, actionName );
        }

        private void SetupWorkflow( string entityName, string deleteTypeGuid, string saveTypeGuid, string deleteActivityGuid, string saveActivityGuid, string deleteActionGuid, string saveActionGuid )
        {
            RockMigrationHelper.UpdateWorkflowType( false, true, string.Format( "Apollos {0} Delete Workflow Type", entityName ), "", categoryGuid, "Work", "fa fa-trash-o", 0, true, 0, deleteTypeGuid );
            RockMigrationHelper.UpdateWorkflowType( false, true, string.Format( "Apollos {0} Save Workflow Type", entityName ), "", categoryGuid, "Work", "fa fa-floppy-o", 0, true, 0, saveTypeGuid );

            RockMigrationHelper.UpdateWorkflowActivityType( deleteTypeGuid, true, string.Format( "Apollos {0} Delete Activity", entityName ), "", true, 0, deleteActivityGuid );
            RockMigrationHelper.UpdateWorkflowActivityType( saveTypeGuid, true, string.Format( "Apollos {0} Save Activity", entityName ), "", true, 0, saveActivityGuid );

            RockMigrationHelper.UpdateWorkflowActionType( deleteActivityGuid, string.Format( "Apollos {0} Delete Action", entityName ), 0, apiSyncGuid, true, false, "", "", 1, "", deleteActionGuid );
            RockMigrationHelper.UpdateWorkflowActionType( saveActivityGuid, string.Format( "Apollos {0} Save Action", entityName ), 0, apiSyncGuid, true, false, "", "", 1, "", saveActionGuid );

            SetupAttributeValues( deleteActionGuid, "Delete", entityName );
            SetupAttributeValues( saveActionGuid, "Save", entityName );
        }

        private void CreateTriggers( string entityName, string qualifierColumn, string qualifierValue, string saveTypeGuid, string deleteTypeGuid )
        {
            CreateSingleTrigger( entityName, WorkflowTriggerType.ImmediatePostSave, qualifierColumn, qualifierValue, saveTypeGuid, string.Format( "{0} Save", entityName ) );
            CreateSingleTrigger( entityName, WorkflowTriggerType.PreDelete, qualifierColumn, qualifierValue, deleteTypeGuid, string.Format( "{0} Delete", entityName ) );
        }

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "cc.newspring.Apollos.Workflow.Action.APISync", apiSyncGuid, false, true );
            RockMigrationHelper.UpdateEntityType( "cc.newspring.Apollos.Security.Authentication.Apollos", apollosAuthGuid, false, true );

            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", activeAttributeGuid ); // Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Action", "Action", "The workflow that this action is under is triggered by what type of event", 0, @"", actionAttributeGuid ); // Action
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Sync URL", "SyncURL", "The specific URL endpoint this related entity type should synchronize with", 0, @"", syncAttributeGuid ); // Sync URL
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Name", "TokenName", "The key by which the token should be identified in the header of HTTP requests", 0, @"", tokenNameAttributeGuid ); // Token Name
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Value", "TokenValue", "The value of the token to authenticate with the URL endpoint", 0, @"", tokenValueAttributeGuid ); // Token Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", orderAttributeGuid ); // Order

            RockMigrationHelper.UpdateCategory( "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE", "Apollos API Sync Category", "fa fa-connectdevelop", "", categoryGuid, 0 );

            var entityName = "UserLogin";
            SetupWorkflow( entityName, userDeleteTypeGuid, userSaveTypeGuid, userDeleteActivityGuid, userSaveActivityGuid, userDeleteActionGuid, userSaveActionGuid );
            var qualifierValue = string.Format( "(SELECT Id FROM EntityType WHERE Guid = '{0}')", apollosAuthGuid );
            CreateTriggers( entityName, "EntityTypeId", qualifierValue, userSaveTypeGuid, userDeleteTypeGuid );

            entityName = "Person";
            SetupWorkflow( entityName, personDeleteTypeGuid, personSaveTypeGuid, personDeleteActivityGuid, personSaveActivityGuid, personDeleteActionGuid, personSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", personSaveTypeGuid, personDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteTriggersByCategory( categoryGuid );

            RockMigrationHelper.DeleteEntityType( apollosAuthGuid );

            DeleteAttributeValuesByAction( userSaveActionGuid );
            DeleteAttributeValuesByAction( userDeleteActionGuid );
            DeleteAttributeValuesByAction( personSaveActionGuid );
            DeleteAttributeValuesByAction( personDeleteActionGuid );

            DeleteWorkflowActionType( userSaveActionGuid );
            DeleteWorkflowActionType( userDeleteActionGuid );
            DeleteWorkflowActionType( personSaveActionGuid );
            DeleteWorkflowActionType( personDeleteActionGuid );

            DeleteWorkflowActivityType( userSaveActivityGuid );
            DeleteWorkflowActivityType( userDeleteActivityGuid );
            DeleteWorkflowActivityType( personSaveActivityGuid );
            DeleteWorkflowActivityType( personDeleteActivityGuid );

            DeleteWorkflowType( userSaveTypeGuid );
            DeleteWorkflowType( userDeleteTypeGuid );
            DeleteWorkflowType( personSaveTypeGuid );
            DeleteWorkflowType( personDeleteTypeGuid );

            RockMigrationHelper.DeleteCategory( categoryGuid );

            DeleteAttributesByEntity( apiSyncGuid );

            RockMigrationHelper.DeleteEntityType( apiSyncGuid );
        }
    }
}