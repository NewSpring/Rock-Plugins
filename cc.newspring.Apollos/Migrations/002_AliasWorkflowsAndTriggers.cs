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
    [MigrationNumber( 2, "1.2.0" )]
    public class AliasWorkflowsAndTriggers : ApollosMigration
    {
        private string aliasDeleteTypeGuid = "22C48750-27DC-4148-840F-9C0AC5383941";
        private string aliasSaveTypeGuid = "56FB5805-890D-4438-9132-1CDF9FBBC921";
        private string aliasDeleteActivityGuid = "1CE2D3B3-53D7-430C-BEFE-0A03C69C3182";
        private string aliasSaveActivityGuid = "25D83BD2-5C91-4877-9523-A5E4A2DF8FE9";
        private string aliasDeleteActionGuid = "233A88F3-FDD9-4302-9F9C-4ED15859D720";
        private string aliasSaveActionGuid = "E9CE1B54-27DD-46F8-91EA-AB0C00F4D7C3";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            var entityName = "PersonAlias";
            SetupWorkflow( entityName, aliasDeleteTypeGuid, aliasSaveTypeGuid, aliasDeleteActivityGuid, aliasSaveActivityGuid, aliasDeleteActionGuid, aliasSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", aliasSaveTypeGuid, aliasDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteAttributeValuesByAction( aliasSaveActionGuid );
            DeleteAttributeValuesByAction( aliasDeleteActionGuid );
            
            DeleteWorkflowActionType( aliasSaveActionGuid );
            DeleteWorkflowActionType( aliasDeleteActionGuid );
           
            DeleteWorkflowActivityType( aliasSaveActivityGuid );
            DeleteWorkflowActivityType( aliasDeleteActivityGuid );
            
            DeleteWorkflowType( aliasSaveTypeGuid );
            DeleteWorkflowType( aliasDeleteTypeGuid );
        }
    }
}