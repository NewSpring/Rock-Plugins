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
    [MigrationNumber( 3, "1.2.0" )]
    public class GroupWorkflowsAndTriggers : Migration
    {
        private string categoryGuid = "65CCE790-BE3D-4C52-AB4C-0BA8FFEE630E";
        private string apiSyncGuid = "6AD93C7E-E314-4618-8EC2-9A8DF9AEAE61";

        private string groupDeleteTypeGuid = "DED93E13-6D61-4777-9D2A-9A158937AF09";
        private string groupSaveTypeGuid = "FF6A6BA7-2505-4E6E-A8B1-E62926028341";
        private string groupDeleteActivityGuid = "DD52B343-1736-452A-B277-04498460B287";
        private string groupSaveActivityGuid = "6B333235-281B-405B-8630-DFAF6C45DCD4";
        private string groupDeleteActionGuid = "4EB50AFD-61B8-4EE5-8A8B-B620416C8BEB";
        private string groupSaveActionGuid = "4BF32E8C-FBC5-457F-8BD7-EC5895C9118F";

        private string groupTypeDeleteTypeGuid = "C821848F-29A3-44DD-B25F-809DFE3013CA";
        private string groupTypeSaveTypeGuid = "BF8F3C8D-0F87-4639-8BD8-CB2D21F3AB57";
        private string groupTypeDeleteActivityGuid = "8906F99F-E3AC-4410-B71F-EB324C5B26EF";
        private string groupTypeSaveActivityGuid = "FA855CB3-5CAF-41DE-9707-A05CEA3C3DB9";
        private string groupTypeDeleteActionGuid = "FD6DFC04-BA3C-488B-9258-A004B20E4587";
        private string groupTypeSaveActionGuid = "64E56BD1-04F2-4A40-9E8E-FBABC7393047";

        private string groupMemberDeleteTypeGuid = "C42C417A-F9B3-4763-A66A-2A0307CBE61C";
        private string groupMemberSaveTypeGuid = "A0972BBA-72F1-4AE0-83A3-497E14E66956";
        private string groupMemberDeleteActivityGuid = "E25AE4AE-F59B-483E-965C-5767BD355F3E";
        private string groupMemberSaveActivityGuid = "74FC30A9-944E-4A65-9698-2B22307411B0";
        private string groupMemberDeleteActionGuid = "60408DAA-F1A7-41DF-899B-D209FB0F2313";
        private string groupMemberSaveActionGuid = "7D09A468-D3C6-4369-8D28-D928BC0F6AD6";

        private string campusDeleteTypeGuid = "6EF12494-5E44-42D2-BA37-0D3EAC732798";
        private string campusSaveTypeGuid = "3FD71478-21FB-4D9C-966B-3DB6CDCCEBB9";
        private string campusDeleteActivityGuid = "69455B87-03CF-4A54-B78C-25CF926D6A78";
        private string campusSaveActivityGuid = "5ED542AF-1982-4C2B-8E4F-18CE46B03ADC";
        private string campusDeleteActionGuid = "73331456-7820-48C7-A80C-B5B03EA86624";
        private string campusSaveActionGuid = "82610243-1A1E-45DF-86C1-51C30E090914";

        private string activeAttributeGuid = "65CB1840-9F36-4369-AC8E-7AB94BF18D1B";
        private string actionAttributeGuid = "B5B78DE9-41ED-4175-9DF5-E3E62ADEC388";
        private string syncUrlAttributeGuid = "C166C5D7-FE59-45ED-B38A-5B7B08124CF2";
        private string tokenNameAttributeGuid = "3AF3C584-8687-495C-A474-8568AF5D44B4";
        private string tokenValueAttributeGuid = "8DF06A26-0544-436F-8034-BF86C465AD2B";
        private string restUserAttributeGuid = "120B0006-D1C9-42FA-A4E4-4039D7AF2C5B";
        private string orderAttributeGuid = "B3B900AB-27A0-4573-8144-0CEC65C0C381";

        private string restPersonGuid = "2A8F8AF4-C2A3-454C-B5C3-6482E255B89B";
        private string restUserGuid = "206B766F-539D-47BB-8613-77B64256E09F";
        private string restPersonAliasGuid = "F89F9ED9-E2D5-42D5-92F1-92C233FE61BB";

        private void DeleteTriggersByCategory( string guid )
        {
            Sql( string.Format( @"
                DELETE
                FROM WorkflowTrigger
                WHERE WorkflowTypeId IN (
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
                   ,'{5} API Sync'
                   ,NEWID()
                   ,1)", modelName, qualifierColumn, qualifierValue, workflowTypeGuid, (int)triggerType, description );

            Sql( insertQuery );
        }

        private void DeleteAttributesByEntity( string guid )
        {
            Sql( string.Format( @"
                DELETE
                FROM Attribute
                WHERE EntityTypeId IN (
	                SELECT w.Id
	                FROM EntityType w
	                WHERE w.Guid = '{0}'
                )", guid ) );
        }

        private void DeleteAttributeValuesByAction( string guid )
        {
            Sql( string.Format( @"
                DELETE
                FROM AttributeValue
                WHERE EntityId IN (
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
            Sql( string.Format( "DELETE WorkFlowActivity WHERE [ActivityTypeId] IN (SELECT Id FROM WorkflowActivityType WHERE Guid = '{0}')", guid ) );
            DeleteByGuid( guid, "WorkflowActivityType" );
        }

        private void DeleteWorkflowActionType( string guid )
        {
            Sql( string.Format( "DELETE WorkFlowAction WHERE [ActionTypeId] IN (SELECT Id FROM WorkflowActionType WHERE Guid = '{0}')", guid ) );
            DeleteByGuid( guid, "WorkflowActionType" );
        }

        private void SetupAttributeValues( string actionGuid, string actionName, string entityName )
        {
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, syncUrlAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenValueAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenNameAttributeGuid, @"apollos" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, orderAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, activeAttributeGuid, @"False" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, restUserAttributeGuid, restPersonAliasGuid );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, actionAttributeGuid, actionName );
        }

        private void SetupWorkflow( string entityName, string deleteTypeGuid, string saveTypeGuid, string deleteActivityGuid, string saveActivityGuid, string deleteActionGuid, string saveActionGuid )
        {
            RockMigrationHelper.UpdateWorkflowType( false, true, string.Format( "{0} Delete Workflow Type", entityName ), "", categoryGuid, "Work", "fa fa-trash-o", 0, true, 0, deleteTypeGuid );
            RockMigrationHelper.UpdateWorkflowType( false, true, string.Format( "{0} Save Workflow Type", entityName ), "", categoryGuid, "Work", "fa fa-floppy-o", 0, true, 0, saveTypeGuid );

            RockMigrationHelper.UpdateWorkflowActivityType( deleteTypeGuid, true, string.Format( "{0} Delete Activity", entityName ), "", true, 0, deleteActivityGuid );
            RockMigrationHelper.UpdateWorkflowActivityType( saveTypeGuid, true, string.Format( "{0} Save Activity", entityName ), "", true, 0, saveActivityGuid );

            RockMigrationHelper.UpdateWorkflowActionType( deleteActivityGuid, string.Format( "{0} Delete Action", entityName ), 0, apiSyncGuid, true, false, "", "", 1, "", deleteActionGuid );
            RockMigrationHelper.UpdateWorkflowActionType( saveActivityGuid, string.Format( "{0} Save Action", entityName ), 0, apiSyncGuid, true, false, "", "", 1, "", saveActionGuid );

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
            var entityName = "Group";
            SetupWorkflow( entityName, groupDeleteTypeGuid, groupSaveTypeGuid, groupDeleteActivityGuid, groupSaveActivityGuid, groupDeleteActionGuid, groupSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", groupSaveTypeGuid, groupDeleteTypeGuid );

            entityName = "GroupType";
            SetupWorkflow( entityName, groupTypeDeleteTypeGuid, groupTypeSaveTypeGuid, groupTypeDeleteActivityGuid, groupTypeSaveActivityGuid, groupTypeDeleteActionGuid, groupTypeSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", groupTypeSaveTypeGuid, groupTypeDeleteTypeGuid );

            entityName = "GroupMember";
            SetupWorkflow( entityName, groupMemberDeleteTypeGuid, groupMemberSaveTypeGuid, groupMemberDeleteActivityGuid, groupMemberSaveActivityGuid, groupMemberDeleteActionGuid, groupMemberSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", groupMemberSaveTypeGuid, groupMemberDeleteTypeGuid );

            entityName = "Campus";
            SetupWorkflow( entityName, campusDeleteTypeGuid, campusSaveTypeGuid, campusDeleteActivityGuid, campusSaveActivityGuid, campusDeleteActionGuid, campusSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", campusSaveTypeGuid, campusDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteAttributeValuesByAction( groupSaveActionGuid );
            DeleteAttributeValuesByAction( groupDeleteActionGuid );
            DeleteAttributeValuesByAction( groupTypeSaveActionGuid );
            DeleteAttributeValuesByAction( groupTypeDeleteActionGuid );
            DeleteAttributeValuesByAction( groupMemberSaveActionGuid );
            DeleteAttributeValuesByAction( groupMemberDeleteActionGuid );
            DeleteAttributeValuesByAction( campusSaveActionGuid );
            DeleteAttributeValuesByAction( campusDeleteActionGuid );

            DeleteWorkflowActionType( groupSaveActionGuid );
            DeleteWorkflowActionType( groupDeleteActionGuid );
            DeleteWorkflowActionType( groupTypeSaveActionGuid );
            DeleteWorkflowActionType( groupTypeDeleteActionGuid );
            DeleteWorkflowActionType( groupMemberSaveActionGuid );
            DeleteWorkflowActionType( groupMemberDeleteActionGuid );
            DeleteWorkflowActionType( campusSaveActionGuid );
            DeleteWorkflowActionType( campusDeleteActionGuid );

            DeleteWorkflowActivityType( groupSaveActivityGuid );
            DeleteWorkflowActivityType( groupDeleteActivityGuid );
            DeleteWorkflowActivityType( groupTypeSaveActivityGuid );
            DeleteWorkflowActivityType( groupTypeDeleteActivityGuid );
            DeleteWorkflowActivityType( groupMemberSaveActivityGuid );
            DeleteWorkflowActivityType( groupMemberDeleteActivityGuid );
            DeleteWorkflowActivityType( campusSaveActivityGuid );
            DeleteWorkflowActivityType( campusDeleteActivityGuid );

            DeleteWorkflowType( groupSaveTypeGuid );
            DeleteWorkflowType( groupDeleteTypeGuid );
            DeleteWorkflowType( groupTypeSaveTypeGuid );
            DeleteWorkflowType( groupTypeDeleteTypeGuid );
            DeleteWorkflowType( groupMemberSaveTypeGuid );
            DeleteWorkflowType( groupMemberDeleteTypeGuid );
            DeleteWorkflowType( campusSaveTypeGuid );
            DeleteWorkflowType( campusDeleteTypeGuid );
        }
    }
}