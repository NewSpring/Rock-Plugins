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
    [MigrationNumber( 28, "1.3.0" )]
    public class FinancialTransactionReceipt : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.ActivateActivity", "38907A90-1634-4A93-8017-619326A4A582", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.PersistWorkflow", "F1A39347-6FE0-43D4-89FB-544195088ECF", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.RunSQL", "A41216D6-6FB0-4019-B222-2C29B4519CF4", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SendEmail", "66197B01-D1F0-4924-A315-47AD54E030DE", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeFromEntity", "972F19B9-598B-474B-97A4-50E56E7B59D2", false, true );
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "E8ABD802-372C-47BE-82B1-96F50DB5169E" ); // Rock.Workflow.Action.ActivateActivity:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "739FD425-5B8C-4605-B775-7E4D9D4C11DB", "Activity", "Activity", "The activity type to activate", 0, @"", "02D5A7A5-8781-46B4-B9FC-AF816829D240" ); // Rock.Workflow.Action.ActivateActivity:Activity
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "3809A78C-B773-440C-8E3F-A8E81D0DAE08" ); // Rock.Workflow.Action.ActivateActivity:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Body", "Body", "The body of the email that should be sent. <span class='tip tip-lava'></span> <span class='tip tip-html'></span>", 3, @"", "4D245B9E-6B03-46E7-8482-A51FBA190E4D" ); // Rock.Workflow.Action.SendEmail:Body
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "36197160-7D3D-490D-AB42-7E29105AFE91" ); // Rock.Workflow.Action.SendEmail:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "From Email Address|Attribute Value", "From", "The email address or an attribute that contains the person or email address that email should be sent from (will default to organization email). <span class='tip tip-lava'></span>", 0, @"", "9F5F7CEC-F369-4FDF-802A-99074CE7A7FC" ); // Rock.Workflow.Action.SendEmail:From Email Address|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Send To Email Address|Attribute Value", "To", "The email address or an attribute that contains the person or email address that email should be sent to. <span class='tip tip-lava'></span>", 1, @"", "0C4C13B8-7076-4872-925A-F950886B5E16" ); // Rock.Workflow.Action.SendEmail:Send To Email Address|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Subject", "Subject", "The subject that should be used when sending email. <span class='tip tip-lava'></span>", 2, @"", "5D9B13B6-CD96-4C7C-86FA-4512B9D28386" ); // Rock.Workflow.Action.SendEmail:Subject
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "D1269254-C15A-40BD-B784-ADCC231D3950" ); // Rock.Workflow.Action.SendEmail:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Lava Template", "LavaTemplate", "By default this action will set the attribute value equal to the guid (or id) of the entity that was passed in for processing. If you include a lava template here, the action will instead set the attribute value to the output of this template. The mergefield to use for the entity is 'Entity.' For example, use {{ Entity.Name }} if the entity has a Name property. <span class='tip tip-lava'></span>", 4, @"", "44836A7C-5007-4213-9AE7-E787225659C3" ); // Rock.Workflow.Action.SetAttributeFromEntity:Lava Template
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1" ); // Rock.Workflow.Action.SetAttributeFromEntity:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Entity Is Required", "EntityIsRequired", "Should an error be returned if the entity is missing or not a valid entity type?", 2, @"True", "05B2AC7A-6C94-41C0-BF92-EE3DEF7895B1" ); // Rock.Workflow.Action.SetAttributeFromEntity:Entity Is Required
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Use Id instead of Guid", "UseId", "Most entity attribute field types expect the Guid of the entity (which is used by default). Select this option if the entity's Id should be used instead (should be rare).", 3, @"False", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B" ); // Rock.Workflow.Action.SetAttributeFromEntity:Use Id instead of Guid
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attribute", "Attribute", "The attribute to set the value of.", 1, @"", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7" ); // Rock.Workflow.Action.SetAttributeFromEntity:Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "AD4EFAC4-E687-43DF-832F-0DC3856ABABB" ); // Rock.Workflow.Action.SetAttributeFromEntity:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "A41216D6-6FB0-4019-B222-2C29B4519CF4", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "SQLQuery", "SQLQuery", "The SQL query to run. <span class='tip tip-lava'></span>", 0, @"", "F3B9908B-096F-460B-8320-122CF046D1F9" ); // Rock.Workflow.Action.RunSQL:SQLQuery
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "A41216D6-6FB0-4019-B222-2C29B4519CF4", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "A18C3143-0586-4565-9F36-E603BC674B4E" ); // Rock.Workflow.Action.RunSQL:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "A41216D6-6FB0-4019-B222-2C29B4519CF4", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Result Attribute", "ResultAttribute", "An optional attribute to set to the scaler result of SQL query.", 1, @"", "56997192-2545-4EA1-B5B2-313B04588984" ); // Rock.Workflow.Action.RunSQL:Result Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "A41216D6-6FB0-4019-B222-2C29B4519CF4", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "FA7C685D-8636-41EF-9998-90FFF3998F76" ); // Rock.Workflow.Action.RunSQL:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "50B01639-4938-40D2-A791-AA0EB4F86847" ); // Rock.Workflow.Action.PersistWorkflow:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Persist Immediately", "PersistImmediately", "This action will normally cause the workflow to be persisted (saved) once all the current activites/actions have completed processing. Set this flag to true, if the workflow should be persisted immediately. This is only required if a subsequent action needs a persisted workflow with a valid id.", 0, @"False", "CB7BF538-99A2-4BAE-B848-D96269FB541C" ); // Rock.Workflow.Action.PersistWorkflow:Persist Immediately
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "86F795B0-0CB6-4DA4-9CE4-B11D0922F361" ); // Rock.Workflow.Action.PersistWorkflow:Order
            RockMigrationHelper.UpdateWorkflowType( false, true, "Send Give Receipt", "", "78E38655-D951-41DB-A0FF-D6474775CFA1", "SendGiveReceipt", "fa fa-dollar", 0, false, 0, "DEDC744B-8112-4374-B4A1-4BA8462C4938" ); // Send Give Receipt
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "DEDC744B-8112-4374-B4A1-4BA8462C4938", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Transaction GUID", "TransactionGUID", "", 0, @"", "C8E8013E-8492-4A5B-A9FF-3F6F83FD1C1A" ); // Send Give Receipt:Transaction GUID
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "DEDC744B-8112-4374-B4A1-4BA8462C4938", "9C204CD0-1233-41C5-818A-C5DA439445AA", "User Email Address", "UserEmailAddress", "", 2, @"", "BFDC8B6E-F8F9-42CA-A9FE-E464AFD4506C" ); // Send Give Receipt:User Email Address
            RockMigrationHelper.AddAttributeQualifier( "C8E8013E-8492-4A5B-A9FF-3F6F83FD1C1A", "ispassword", @"False", "977D858C-5B52-4380-9A71-67932878561F" ); // Send Give Receipt:Transaction GUID:ispassword
            RockMigrationHelper.AddAttributeQualifier( "BFDC8B6E-F8F9-42CA-A9FE-E464AFD4506C", "ispassword", @"False", "171C7FA1-A19D-4487-8400-5E03F63D3A48" ); // Send Give Receipt:User Email Address:ispassword
            RockMigrationHelper.UpdateWorkflowActivityType( "DEDC744B-8112-4374-B4A1-4BA8462C4938", true, "Get Transaction GUID", "", true, 0, "0B2F6C6C-AF8C-46CB-9D79-799F8A9D6CB6" ); // Send Give Receipt:Get Transaction GUID
            RockMigrationHelper.UpdateWorkflowActivityType( "DEDC744B-8112-4374-B4A1-4BA8462C4938", true, "Get User ID", "", false, 1, "3E124741-2EE4-4396-B5D8-C3C6FDC2FFF5" ); // Send Give Receipt:Get User ID
            RockMigrationHelper.UpdateWorkflowActivityType( "DEDC744B-8112-4374-B4A1-4BA8462C4938", true, "Send Email", "", false, 2, "349C67B4-E7BF-402D-AB39-BB4FB922F6DB" ); // Send Give Receipt:Send Email
            RockMigrationHelper.UpdateWorkflowActionType( "0B2F6C6C-AF8C-46CB-9D79-799F8A9D6CB6", "Get Transaction GUID", 0, "972F19B9-598B-474B-97A4-50E56E7B59D2", true, false, "", "", 1, "", "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID
            RockMigrationHelper.UpdateWorkflowActionType( "3E124741-2EE4-4396-B5D8-C3C6FDC2FFF5", "Get User ID", 0, "A41216D6-6FB0-4019-B222-2C29B4519CF4", true, false, "", "", 1, "", "A78C1FD3-F0EC-4B45-8DC7-40362875438F" ); // Send Give Receipt:Get User ID:Get User ID
            RockMigrationHelper.UpdateWorkflowActionType( "0B2F6C6C-AF8C-46CB-9D79-799F8A9D6CB6", "Persist Workflow", 1, "F1A39347-6FE0-43D4-89FB-544195088ECF", true, false, "", "", 1, "", "886C247A-3A35-47D8-B324-D6AF90B76219" ); // Send Give Receipt:Get Transaction GUID:Persist Workflow
            RockMigrationHelper.UpdateWorkflowActionType( "0B2F6C6C-AF8C-46CB-9D79-799F8A9D6CB6", "Activate Activity", 2, "38907A90-1634-4A93-8017-619326A4A582", true, true, "", "", 1, "", "D4D93F0D-41B7-4796-B1C1-C68841AAAFB4" ); // Send Give Receipt:Get Transaction GUID:Activate Activity
            RockMigrationHelper.UpdateWorkflowActionType( "349C67B4-E7BF-402D-AB39-BB4FB922F6DB", "Send Email", 0, "66197B01-D1F0-4924-A315-47AD54E030DE", true, false, "", "", 1, "", "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516" ); // Send Give Receipt:Send Email:Send Email
            RockMigrationHelper.UpdateWorkflowActionType( "3E124741-2EE4-4396-B5D8-C3C6FDC2FFF5", "Activate Activity", 1, "38907A90-1634-4A93-8017-619326A4A582", true, true, "", "", 1, "", "F18CA344-1F31-4691-BE2B-AE1C36F8CBD5" ); // Send Give Receipt:Get User ID:Activate Activity
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "AD4EFAC4-E687-43DF-832F-0DC3856ABABB", @"" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1", @"False" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7", @"c8e8013e-8492-4a5b-a9ff-3f6f83fd1c1a" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "05B2AC7A-6C94-41C0-BF92-EE3DEF7895B1", @"True" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Entity Is Required
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B", @"False" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Use Id instead of Guid
            RockMigrationHelper.AddActionTypeAttributeValue( "569FD7AF-4B50-494A-B7AF-C2AAC2C467E1", "44836A7C-5007-4213-9AE7-E787225659C3", @"" ); // Send Give Receipt:Get Transaction GUID:Get Transaction GUID:Lava Template
            RockMigrationHelper.AddActionTypeAttributeValue( "886C247A-3A35-47D8-B324-D6AF90B76219", "50B01639-4938-40D2-A791-AA0EB4F86847", @"False" ); // Send Give Receipt:Get Transaction GUID:Persist Workflow:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "886C247A-3A35-47D8-B324-D6AF90B76219", "86F795B0-0CB6-4DA4-9CE4-B11D0922F361", @"" ); // Send Give Receipt:Get Transaction GUID:Persist Workflow:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "886C247A-3A35-47D8-B324-D6AF90B76219", "CB7BF538-99A2-4BAE-B848-D96269FB541C", @"False" ); // Send Give Receipt:Get Transaction GUID:Persist Workflow:Persist Immediately
            RockMigrationHelper.AddActionTypeAttributeValue( "D4D93F0D-41B7-4796-B1C1-C68841AAAFB4", "E8ABD802-372C-47BE-82B1-96F50DB5169E", @"False" ); // Send Give Receipt:Get Transaction GUID:Activate Activity:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "D4D93F0D-41B7-4796-B1C1-C68841AAAFB4", "02D5A7A5-8781-46B4-B9FC-AF816829D240", @"3E124741-2EE4-4396-B5D8-C3C6FDC2FFF5" ); // Send Give Receipt:Get Transaction GUID:Activate Activity:Activity
            RockMigrationHelper.AddActionTypeAttributeValue( "D4D93F0D-41B7-4796-B1C1-C68841AAAFB4", "3809A78C-B773-440C-8E3F-A8E81D0DAE08", @"" ); // Send Give Receipt:Get Transaction GUID:Activate Activity:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "A78C1FD3-F0EC-4B45-8DC7-40362875438F", "A18C3143-0586-4565-9F36-E603BC674B4E", @"False" ); // Send Give Receipt:Get User ID:Get User ID:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "A78C1FD3-F0EC-4B45-8DC7-40362875438F", "FA7C685D-8636-41EF-9998-90FFF3998F76", @"" ); // Send Give Receipt:Get User ID:Get User ID:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "A78C1FD3-F0EC-4B45-8DC7-40362875438F", "F3B9908B-096F-460B-8320-122CF046D1F9", @"SELECT Email FROM Person INNER JOIN  (     SELECT PersonId     FROM PersonAlias     INNER JOIN (         SELECT              AuthorizedPersonAliasId         FROM              FinancialTransaction         Where              GUID='{{ Workflow.TransactionGUID }}'     ) Aliases     ON PersonAlias.Id = Aliases.AuthorizedPersonAliasId ) Results ON Person.Id = Results.PersonId  " ); // Send Give Receipt:Get User ID:Get User ID:SQLQuery
            RockMigrationHelper.AddActionTypeAttributeValue( "A78C1FD3-F0EC-4B45-8DC7-40362875438F", "56997192-2545-4EA1-B5B2-313B04588984", @"bfdc8b6e-f8f9-42ca-a9fe-e464afd4506c" ); // Send Give Receipt:Get User ID:Get User ID:Result Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "F18CA344-1F31-4691-BE2B-AE1C36F8CBD5", "E8ABD802-372C-47BE-82B1-96F50DB5169E", @"False" ); // Send Give Receipt:Get User ID:Activate Activity:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "F18CA344-1F31-4691-BE2B-AE1C36F8CBD5", "02D5A7A5-8781-46B4-B9FC-AF816829D240", @"349C67B4-E7BF-402D-AB39-BB4FB922F6DB" ); // Send Give Receipt:Get User ID:Activate Activity:Activity
            RockMigrationHelper.AddActionTypeAttributeValue( "F18CA344-1F31-4691-BE2B-AE1C36F8CBD5", "3809A78C-B773-440C-8E3F-A8E81D0DAE08", @"" ); // Send Give Receipt:Get User ID:Activate Activity:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "36197160-7D3D-490D-AB42-7E29105AFE91", @"False" ); // Send Give Receipt:Send Email:Send Email:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "9F5F7CEC-F369-4FDF-802A-99074CE7A7FC", @"hello@newspring.cc" ); // Send Give Receipt:Send Email:Send Email:From Email Address|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "D1269254-C15A-40BD-B784-ADCC231D3950", @"" ); // Send Give Receipt:Send Email:Send Email:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "0C4C13B8-7076-4872-925A-F950886B5E16", @"bfdc8b6e-f8f9-42ca-a9fe-e464afd4506c" ); // Send Give Receipt:Send Email:Send Email:Send To Email Address|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "5D9B13B6-CD96-4C7C-86FA-4512B9D28386", @"Success!" ); // Send Give Receipt:Send Email:Send Email:Subject
            RockMigrationHelper.AddActionTypeAttributeValue( "AC48ADD7-54A9-4CDD-A5FB-5FEF43547516", "4D245B9E-6B03-46E7-8482-A51FBA190E4D", @"Successful email!" ); // Send Give Receipt:Send Email:Send Email:Body
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.DeleteWorkflowType( "DEDC744B-8112-4374-B4A1-4BA8462C4938" );
        }
    }
}