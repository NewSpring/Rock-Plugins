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

namespace cc.newspring.Workflows.Migrations
{
    [MigrationNumber( 1, "1.2.0" )]
    public class Wufoo : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true );
            RockMigrationHelper.UpdateWorkflowType( false, true, "Testing", "This is a test workflow", "78E38655-D951-41DB-A0FF-D6474775CFA1", "Work", "fa fa-list-ol", 0, true, 0, "C95EF41F-8054-4873-BC4B-E29E97F1540D" ); // Testing
            RockMigrationHelper.UpdateWorkflowActivityType( "C95EF41F-8054-4873-BC4B-E29E97F1540D", true, "Test Action", "This is a test action", false, 0, "F31DBE63-796B-440B-BDE3-616D14736B7B" ); // Testing:Test Action
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}