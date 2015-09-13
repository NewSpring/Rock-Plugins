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
    public class SetupWorkflowsAndTriggers : ApollosMigration
    {
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

        private string accountDeleteTypeGuid = "12C48750-27CC-4148-840F-9C0AC5383941";
        private string accountSaveTypeGuid = "46FB5805-890B-4438-9132-1CDF9FBBC921";
        private string accountDeleteActivityGuid = "0CE263B3-53D7-430C-BEFE-0A03C69C3182";
        private string accountSaveActivityGuid = "55D83B32-5C91-4877-9523-A5E4A2DF8FE9";
        private string accountDeleteActionGuid = "133A88F3-FD89-4302-9F9C-4ED15859D720";
        private string accountSaveActionGuid = "F9CE1B54-27D2-46F8-91EA-AB0C00F4D7C3";

        private string scheduledDeleteTypeGuid = "631D9D0D-9C34-4EEC-9167-254E7EE27EB1";
        private string scheduledSaveTypeGuid = "890AE615-0927-460F-AE9B-423CF2DE23B8";
        private string scheduledDeleteActivityGuid = "6A1D2B99-F41D-421F-8D58-7CE33A7FE1F5";
        private string scheduledSaveActivityGuid = "A7B4201F-E7A3-46D1-8877-84E00ED27A80";
        private string scheduledDeleteActionGuid = "42674CD5-C9DF-4334-88BA-5B747810BFA7";
        private string scheduledSaveActionGuid = "C91133B0-AC65-4651-B012-6F452D090511";

        private string scheduledDetailDeleteTypeGuid = "531D9D0D-9C34-4EEC-9167-254E7EE27EB1";
        private string scheduledDetailSaveTypeGuid = "790AE615-0927-460F-AE9B-423CF2DE23B8";
        private string scheduledDetailDeleteActivityGuid = "5A1D2B99-F41D-421F-8D58-7CE33A7FE1F5";
        private string scheduledDetailSaveActivityGuid = "97B4201F-E7A3-46D1-8877-84E00ED27A80";
        private string scheduledDetailDeleteActionGuid = "32674CD5-C9DF-4334-88BA-5B747810BFA7";
        private string scheduledDetailSaveActionGuid = "B91133B0-AC65-4651-B012-6F452D090511";

        private string transactionDeleteTypeGuid = "6D419224-0F09-4C6F-84DD-1DFB680D907F";
        private string transactionSaveTypeGuid = "176DA7CC-2E79-4333-A460-E156DA660828";
        private string transactionDeleteActivityGuid = "37098907-0BBD-4CB2-A018-9DDC4EE214C8";
        private string transactionSaveActivityGuid = "CACABC91-BEEF-4E88-949C-7CADE231225C";
        private string transactionDeleteActionGuid = "0D3168EE-8FEF-476D-93C5-3200F74C2AD0";
        private string transactionSaveActionGuid = "05EE8319-BC8E-4741-9184-BFFFE90BD863";

        private string transactionDetailDeleteTypeGuid = "0F3EF9AE-DC21-4EE0-8326-2AEF6ABDD5F1";
        private string transactionDetailSaveTypeGuid = "85E637C3-BC63-4C14-A5B5-FF794C566A77";
        private string transactionDetailDeleteActivityGuid = "A8896227-5B19-4660-A948-C41A8722577E";
        private string transactionDetailSaveActivityGuid = "D175B5C1-46B8-44CF-AB45-0476F714F947";
        private string transactionDetailDeleteActionGuid = "CB66D12F-0F1E-4305-BEF2-EC492205E6F0";
        private string transactionDetailSaveActionGuid = "072ABA02-3497-4292-A3A1-6697FD970F01";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql( string.Format( @"
                INSERT INTO [dbo].[Person] (
                    [IsSystem]
                   ,[RecordTypeValueId]
                   ,[RecordStatusValueId]
                   ,[IsDeceased]
                   ,[LastName]
                   ,[Gender]
                   ,[Guid]
                   ,[EmailPreference]
                ) VALUES (
                    0
                   ,241
                   ,3
                   ,0
                   ,'Apollos'
                   ,0
                   ,'{0}'
                   ,0)", restPersonGuid ) );

            Sql( string.Format( @"
                INSERT INTO [dbo].[UserLogin] (
                    [UserName]
                   ,[IsConfirmed]
                   ,[ApiKey]
                   ,[PersonId]
                   ,[Guid]
                   ,[EntityTypeId]
                ) VALUES (
                    NEWID()
                   ,1
                   ,NEWID()
                   ,(SELECT [Id] FROM [dbo].[Person] WHERE [Guid] = '{0}')
                   ,'{1}'
                   ,27)", restPersonGuid, restUserGuid ) );

            Sql( string.Format( @"
                INSERT INTO [dbo].[PersonAlias] (
                    [PersonId]
                   ,[AliasPersonId]
                   ,[AliasPersonGuid]
                   ,[Guid]
                ) VALUES (
                    (SELECT [Id] FROM [dbo].[Person] WHERE [Guid] = '{0}')
                   ,(SELECT [Id] FROM [dbo].[Person] WHERE [Guid] = '{0}')
                   ,'{0}'
                   ,'{1}')", restPersonGuid, restPersonAliasGuid ) );

            RockMigrationHelper.UpdateEntityType( "cc.newspring.Apollos.Workflow.Action.APISync", apiSyncGuid, false, true );
            RockMigrationHelper.UpdateEntityType( "cc.newspring.Apollos.Security.Authentication.Apollos", apollosAuthGuid, false, true );

            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", activeAttributeGuid ); // Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Action", "Action", "The workflow that this action is under is triggered by what type of event", 0, @"", actionAttributeGuid ); // Action
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Sync URL", "SyncURL", "The specific URL endpoint this related entity type should synchronize with", 0, @"", syncUrlAttributeGuid ); // Sync URL
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Name", "TokenName", "The key by which the token should be identified in the header of HTTP requests", 0, @"", tokenNameAttributeGuid ); // Token Name
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "9C204CD0-1233-41C5-818A-C5DA439445AA", "Token Value", "TokenValue", "The value of the token to authenticate with the URL endpoint", 0, @"", tokenValueAttributeGuid ); // Token Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Rest User", "RestUser", "The associated REST user that handles sync from the third party", 0, @"", restUserAttributeGuid ); // Rest User
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( apiSyncGuid, "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", orderAttributeGuid ); // Order

            RockMigrationHelper.UpdateCategory( "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE", "API Sync To Apollos", "fa fa-connectdevelop", "", categoryGuid, 0 );

            var entityName = "UserLogin";
            SetupWorkflow( entityName, userDeleteTypeGuid, userSaveTypeGuid, userDeleteActivityGuid, userSaveActivityGuid, userDeleteActionGuid, userSaveActionGuid );
            var qualifierValue = string.Format( "(SELECT Id FROM EntityType WHERE Guid = '{0}')", apollosAuthGuid );
            CreateTriggers( entityName, "EntityTypeId", qualifierValue, userSaveTypeGuid, userDeleteTypeGuid );

            entityName = "Person";
            SetupWorkflow( entityName, personDeleteTypeGuid, personSaveTypeGuid, personDeleteActivityGuid, personSaveActivityGuid, personDeleteActionGuid, personSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", personSaveTypeGuid, personDeleteTypeGuid );

            entityName = "FinancialTransaction";
            SetupWorkflow( entityName, transactionDeleteTypeGuid, transactionSaveTypeGuid, transactionDeleteActivityGuid, transactionSaveActivityGuid, transactionDeleteActionGuid, transactionSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", transactionSaveTypeGuid, transactionDeleteTypeGuid );

            entityName = "FinancialTransactionDetail";
            SetupWorkflow( entityName, transactionDetailDeleteTypeGuid, transactionDetailSaveTypeGuid, transactionDetailDeleteActivityGuid, transactionDetailSaveActivityGuid, transactionDetailDeleteActionGuid, transactionDetailSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", transactionDetailSaveTypeGuid, transactionDetailDeleteTypeGuid );

            entityName = "FinancialAccount";
            SetupWorkflow( entityName, accountDeleteTypeGuid, accountSaveTypeGuid, accountDeleteActivityGuid, accountSaveActivityGuid, accountDeleteActionGuid, accountSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", accountSaveTypeGuid, accountDeleteTypeGuid );

            entityName = "FinancialScheduledTransaction";
            SetupWorkflow( entityName, scheduledDeleteTypeGuid, scheduledSaveTypeGuid, scheduledDeleteActivityGuid, scheduledSaveActivityGuid, scheduledDeleteActionGuid, scheduledSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", scheduledSaveTypeGuid, scheduledDeleteTypeGuid );

            entityName = "FinancialScheduledTransactionDetail";
            SetupWorkflow( entityName, scheduledDetailDeleteTypeGuid, scheduledDetailSaveTypeGuid, scheduledDetailDeleteActivityGuid, scheduledDetailSaveActivityGuid, scheduledDetailDeleteActionGuid, scheduledDetailSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", scheduledDetailSaveTypeGuid, scheduledDetailDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteTriggersByCategory( categoryGuid );

            DeleteAttributeValuesByAction( userSaveActionGuid );
            DeleteAttributeValuesByAction( userDeleteActionGuid );
            DeleteAttributeValuesByAction( transactionSaveActionGuid );
            DeleteAttributeValuesByAction( transactionDeleteActionGuid );
            DeleteAttributeValuesByAction( personSaveActionGuid );
            DeleteAttributeValuesByAction( personDeleteActionGuid );
            DeleteAttributeValuesByAction( transactionDetailSaveActionGuid );
            DeleteAttributeValuesByAction( transactionDetailDeleteActionGuid );
            DeleteAttributeValuesByAction( accountSaveActionGuid );
            DeleteAttributeValuesByAction( accountDeleteActionGuid );
            DeleteAttributeValuesByAction( scheduledSaveActionGuid );
            DeleteAttributeValuesByAction( scheduledDeleteActionGuid );
            DeleteAttributeValuesByAction( scheduledDetailSaveActionGuid );
            DeleteAttributeValuesByAction( scheduledDetailDeleteActionGuid );

            DeleteWorkflowActionType( userSaveActionGuid );
            DeleteWorkflowActionType( userDeleteActionGuid );
            DeleteWorkflowActionType( transactionSaveActionGuid );
            DeleteWorkflowActionType( transactionDeleteActionGuid );
            DeleteWorkflowActionType( personSaveActionGuid );
            DeleteWorkflowActionType( personDeleteActionGuid );
            DeleteWorkflowActionType( transactionDetailSaveActionGuid );
            DeleteWorkflowActionType( transactionDetailDeleteActionGuid );
            DeleteWorkflowActionType( accountSaveActionGuid );
            DeleteWorkflowActionType( accountDeleteActionGuid );
            DeleteWorkflowActionType( scheduledSaveActionGuid );
            DeleteWorkflowActionType( scheduledDeleteActionGuid );
            DeleteWorkflowActionType( scheduledDetailSaveActionGuid );
            DeleteWorkflowActionType( scheduledDetailDeleteActionGuid );

            DeleteWorkflowActivityType( userSaveActivityGuid );
            DeleteWorkflowActivityType( userDeleteActivityGuid );
            DeleteWorkflowActivityType( transactionSaveActivityGuid );
            DeleteWorkflowActivityType( transactionDeleteActivityGuid );
            DeleteWorkflowActivityType( personSaveActivityGuid );
            DeleteWorkflowActivityType( personDeleteActivityGuid );
            DeleteWorkflowActivityType( transactionDetailSaveActivityGuid );
            DeleteWorkflowActivityType( transactionDetailDeleteActivityGuid );
            DeleteWorkflowActivityType( accountSaveActivityGuid );
            DeleteWorkflowActivityType( accountDeleteActivityGuid );
            DeleteWorkflowActivityType( scheduledSaveActivityGuid );
            DeleteWorkflowActivityType( scheduledDeleteActivityGuid );
            DeleteWorkflowActivityType( scheduledDetailSaveActivityGuid );
            DeleteWorkflowActivityType( scheduledDetailDeleteActivityGuid );

            DeleteWorkflowType( userSaveTypeGuid );
            DeleteWorkflowType( userDeleteTypeGuid );
            DeleteWorkflowType( transactionSaveTypeGuid );
            DeleteWorkflowType( transactionDeleteTypeGuid );
            DeleteWorkflowType( personSaveTypeGuid );
            DeleteWorkflowType( personDeleteTypeGuid );
            DeleteWorkflowType( transactionDetailSaveTypeGuid );
            DeleteWorkflowType( transactionDetailDeleteTypeGuid );
            DeleteWorkflowType( accountSaveTypeGuid );
            DeleteWorkflowType( accountDeleteTypeGuid );
            DeleteWorkflowType( scheduledSaveTypeGuid );
            DeleteWorkflowType( scheduledDeleteTypeGuid );
            DeleteWorkflowType( scheduledDetailSaveTypeGuid );
            DeleteWorkflowType( scheduledDetailDeleteTypeGuid );

            RockMigrationHelper.DeleteCategory( categoryGuid );

            DeleteAttributesByEntity( apiSyncGuid );

            RockMigrationHelper.DeleteEntityType( apiSyncGuid );

            Sql( string.Format( @"DELETE FROM [dbo].[UserLogin] WHERE [Guid] = '{0}'", restUserGuid ) );
            Sql( string.Format( @"DELETE FROM PersonAlias WHERE AliasPersonGuid = '{0}'", restPersonGuid ) );
            Sql( string.Format( @"DELETE FROM [dbo].[Person] WHERE [Guid] = '{0}'", restPersonGuid ) );
        }
    }
}