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
    public class GroupWorkflowsAndTriggers : ApollosMigration
    {
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