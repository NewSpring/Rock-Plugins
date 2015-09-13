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
    public abstract class ApollosMigration : Migration
    {
        protected string categoryGuid = "65CCE790-BE3D-4C52-AB4C-0BA8FFEE630E";
        protected string apiSyncGuid = "6AD93C7E-E314-4618-8EC2-9A8DF9AEAE61";

        protected string activeAttributeGuid = "65CB1840-9F36-4369-AC8E-7AB94BF18D1B";
        protected string actionAttributeGuid = "B5B78DE9-41ED-4175-9DF5-E3E62ADEC388";
        protected string syncUrlAttributeGuid = "C166C5D7-FE59-45ED-B38A-5B7B08124CF2";
        protected string tokenNameAttributeGuid = "3AF3C584-8687-495C-A474-8568AF5D44B4";
        protected string tokenValueAttributeGuid = "8DF06A26-0544-436F-8034-BF86C465AD2B";
        protected string restUserAttributeGuid = "120B0006-D1C9-42FA-A4E4-4039D7AF2C5B";
        protected string orderAttributeGuid = "B3B900AB-27A0-4573-8144-0CEC65C0C381";

        protected string restPersonGuid = "2A8F8AF4-C2A3-454C-B5C3-6482E255B89B";
        protected string restUserGuid = "206B766F-539D-47BB-8613-77B64256E09F";
        protected string restPersonAliasGuid = "F89F9ED9-E2D5-42D5-92F1-92C233FE61BB";

        protected void DeleteTriggersByCategory( string guid )
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

        protected void CreateSingleTrigger( string modelName, WorkflowTriggerType triggerType, string qualifierColumn, string qualifierValue, string workflowTypeGuid, string description )
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

        protected void DeleteAttributesByEntity( string guid )
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

        protected void DeleteAttributeValuesByAction( string guid )
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

        protected void DeleteByGuid( string guid, string table )
        {
            Sql( string.Format( "DELETE [{0}] WHERE [Guid] = '{1}'", table, guid ) );
        }

        protected void DeleteWorkflowType( string guid )
        {
            DeleteByGuid( guid, "WorkflowType" );
        }

        protected void DeleteWorkflowActivityType( string guid )
        {
            Sql( string.Format( "DELETE WorkFlowActivity WHERE [ActivityTypeId] IN (SELECT Id FROM WorkflowActivityType WHERE Guid = '{0}')", guid ) );
            DeleteByGuid( guid, "WorkflowActivityType" );
        }

        protected void DeleteWorkflowActionType( string guid )
        {
            Sql( string.Format( "DELETE WorkFlowAction WHERE [ActionTypeId] IN (SELECT Id FROM WorkflowActionType WHERE Guid = '{0}')", guid ) );
            DeleteByGuid( guid, "WorkflowActionType" );
        }

        protected void SetupAttributeValues( string actionGuid, string actionName, string entityName )
        {
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, syncUrlAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenValueAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, tokenNameAttributeGuid, @"apollos" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, orderAttributeGuid, @"" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, activeAttributeGuid, @"False" );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, restUserAttributeGuid, restPersonAliasGuid );
            RockMigrationHelper.AddActionTypeAttributeValue( actionGuid, actionAttributeGuid, actionName );
        }

        protected void SetupWorkflow( string entityName, string deleteTypeGuid, string saveTypeGuid, string deleteActivityGuid, string saveActivityGuid, string deleteActionGuid, string saveActionGuid )
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

        protected void CreateTriggers( string entityName, string qualifierColumn, string qualifierValue, string saveTypeGuid, string deleteTypeGuid )
        {
            CreateSingleTrigger( entityName, WorkflowTriggerType.ImmediatePostSave, qualifierColumn, qualifierValue, saveTypeGuid, string.Format( "{0} Save", entityName ) );
            CreateSingleTrigger( entityName, WorkflowTriggerType.PreDelete, qualifierColumn, qualifierValue, deleteTypeGuid, string.Format( "{0} Delete", entityName ) );
        }
    }
}
