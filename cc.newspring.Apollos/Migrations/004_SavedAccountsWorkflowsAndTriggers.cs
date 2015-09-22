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
    [MigrationNumber( 4, "1.2.0" )]
    public class SavedAccountsWorkflowsAndTriggers : ApollosMigration
    {
        private string savedAccountDeleteTypeGuid = "6E74AB26-6029-46E5-9220-49E2ED284270";
        private string savedAccountSaveTypeGuid = "31E00A5A-A4CA-49FC-9E6C-7E34571B494C";
        private string savedAccountDeleteActivityGuid = "0C86B5A6-714B-4AA4-B1F6-FA4EBE73134F";
        private string savedAccountSaveActivityGuid = "F5BD07CD-23AC-4535-9F19-775C71A0EBF6";
        private string savedAccountDeleteActionGuid = "D11E3347-B0FC-4563-BD0D-23DBE7337E41";
        private string savedAccountSaveActionGuid = "5EE3A534-9F2C-472C-9E97-CE3E17B4A3B2";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            var entityName = "FinancialPersonSavedAccount";
            SetupWorkflow( entityName, savedAccountDeleteTypeGuid, savedAccountSaveTypeGuid, savedAccountDeleteActivityGuid, savedAccountSaveActivityGuid, savedAccountDeleteActionGuid, savedAccountSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", savedAccountSaveTypeGuid, savedAccountDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteAttributeValuesByAction( savedAccountSaveActionGuid );
            DeleteAttributeValuesByAction( savedAccountDeleteActionGuid );

            DeleteWorkflowActionType( savedAccountSaveActionGuid );
            DeleteWorkflowActionType( savedAccountDeleteActionGuid );

            DeleteWorkflowActivityType( savedAccountSaveActivityGuid );
            DeleteWorkflowActivityType( savedAccountDeleteActivityGuid );

            DeleteWorkflowType( savedAccountSaveTypeGuid );
            DeleteWorkflowType( savedAccountDeleteTypeGuid );            
        }
    }
}