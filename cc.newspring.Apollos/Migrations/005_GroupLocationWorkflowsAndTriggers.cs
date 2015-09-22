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
    [MigrationNumber( 5, "1.2.0" )]
    public class GroupLocationWorkflowsAndTriggers : ApollosMigration
    {
        private string groupLocationDeleteTypeGuid = "26F40076-72B0-4920-AE42-78006A40CFC6";
        private string groupLocationSaveTypeGuid = "A0DFF21C-182F-459C-994B-0437317CECC1";
        private string groupLocationDeleteActivityGuid = "D911BBA6-ABB1-4EAA-8CDA-A55F0587A15F";
        private string groupLocationSaveActivityGuid = "450BE677-39FA-4F62-A021-481FBAF2DE22";
        private string groupLocationDeleteActionGuid = "FF7E8FB9-3CD3-441B-82C6-0E352721CD48";
        private string groupLocationSaveActionGuid = "AF5B0DC6-804E-46B0-AF67-E5894267B0E9";

        private string locationDeleteTypeGuid = "1F69A3BB-BD7C-4C42-B924-76722AFEB69C";
        private string locationSaveTypeGuid = "51EF62F0-4566-43DF-958E-0CBC1D4DC6A9";
        private string locationDeleteActivityGuid = "2B942968-3B0E-4FDF-9D21-4089968C749C";
        private string locationSaveActivityGuid = "CC204F4F-DA53-4A17-8271-787E9DF9C953";
        private string locationDeleteActionGuid = "DE144B02-6E01-4E4A-B677-EB4CF74034AC";
        private string locationSaveActionGuid = "B4E15243-2761-451B-BC0B-9B2CAFD1794E";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            var entityName = "GroupLocation";
            SetupWorkflow( entityName, groupLocationDeleteTypeGuid, groupLocationSaveTypeGuid, groupLocationDeleteActivityGuid, groupLocationSaveActivityGuid, groupLocationDeleteActionGuid, groupLocationSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", groupLocationSaveTypeGuid, groupLocationDeleteTypeGuid );

            entityName = "Location";
            SetupWorkflow( entityName, locationDeleteTypeGuid, locationSaveTypeGuid, locationDeleteActivityGuid, locationSaveActivityGuid, locationDeleteActionGuid, locationSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", locationSaveTypeGuid, locationDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteAttributeValuesByAction( locationSaveActionGuid );
            DeleteAttributeValuesByAction( locationDeleteActionGuid );
            DeleteAttributeValuesByAction( groupLocationSaveActionGuid );
            DeleteAttributeValuesByAction( groupLocationDeleteActionGuid );

            DeleteWorkflowActionType( locationSaveActionGuid );
            DeleteWorkflowActionType( locationDeleteActionGuid );
            DeleteWorkflowActionType( groupLocationSaveActionGuid );
            DeleteWorkflowActionType( groupLocationDeleteActionGuid );

            DeleteWorkflowActivityType( locationSaveActivityGuid );
            DeleteWorkflowActivityType( locationDeleteActivityGuid );
            DeleteWorkflowActivityType( groupLocationSaveActivityGuid );
            DeleteWorkflowActivityType( groupLocationDeleteActivityGuid );

            DeleteWorkflowType( locationSaveTypeGuid );
            DeleteWorkflowType( locationDeleteTypeGuid );
            DeleteWorkflowType( groupLocationSaveTypeGuid );
            DeleteWorkflowType( groupLocationDeleteTypeGuid );  
        }
    }
}