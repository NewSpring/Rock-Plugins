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
    [MigrationNumber( 6, "1.2.0" )]
    public class PaymentDetailWorkflowsAndTriggers : ApollosMigration
    {
        private string paymentDetailDeleteTypeGuid = "ED2B4C36-C449-4ECE-A2B5-6820A28B0FAB";
        private string paymentDetailSaveTypeGuid = "3219D250-1004-4F45-B3B8-4829F9140C49";
        private string paymentDetailDeleteActivityGuid = "0342C708-7A56-444C-B192-1B489F1244EA";
        private string paymentDetailSaveActivityGuid = "96749C11-9E1E-4470-A352-D8D31F081FEC";
        private string paymentDetailDeleteActionGuid = "0F63F342-AC93-41E2-B203-157A178CEBFA";
        private string paymentDetailSaveActionGuid = "7F5D2697-24B7-402C-A84E-9EB73CC3B5B3";

        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            var entityName = "FinancialPaymentDetail";
            SetupWorkflow( entityName, paymentDetailDeleteTypeGuid, paymentDetailSaveTypeGuid, paymentDetailDeleteActivityGuid, paymentDetailSaveActivityGuid, paymentDetailDeleteActionGuid, paymentDetailSaveActionGuid );
            CreateTriggers( entityName, string.Empty, "''", paymentDetailSaveTypeGuid, paymentDetailDeleteTypeGuid );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            DeleteAttributeValuesByAction( paymentDetailSaveActionGuid );
            DeleteAttributeValuesByAction( paymentDetailDeleteActionGuid );

            DeleteWorkflowActionType( paymentDetailSaveActionGuid );
            DeleteWorkflowActionType( paymentDetailDeleteActionGuid );

            DeleteWorkflowActivityType( paymentDetailSaveActivityGuid );
            DeleteWorkflowActivityType( paymentDetailDeleteActivityGuid );

            DeleteWorkflowType( paymentDetailSaveTypeGuid );
            DeleteWorkflowType( paymentDetailDeleteTypeGuid );  
        }
    }
}