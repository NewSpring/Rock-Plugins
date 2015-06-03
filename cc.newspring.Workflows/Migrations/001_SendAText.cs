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
    [MigrationNumber( 1, "1.3.0" )]
    public class SendAText : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SendSms", "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeFromEntity", "972F19B9-598B-474B-97A4-50E56E7B59D2", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeToCurrentPerson", "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.UserEntryForm", "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", false, true );
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "DE9CB292-4785-4EA3-976D-3826F91E9E98" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Person Attribute", "PersonAttribute", "The attribute to set to the currently logged in person.", 0, @"", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Person Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "89E9BCED-91AB-47B0-AD52-D78B0B7CB9E8" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "234910F2-A0DB-4D7D-BAF7-83C880EF30AE" ); // Rock.Workflow.Action.UserEntryForm:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "C178113D-7C86-4229-8424-C6D0CF4A7E23" ); // Rock.Workflow.Action.UserEntryForm:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Lava Template", "LavaTemplate", "By default this action will set the attribute value equal to the guid (or id) of the entity that was passed in for processing. If you include a lava template here, the action will instead set the attribute value to the output of this template. The mergefield to use for the entity is 'Entity.' For example, use {{ Entity.Name }} if the entity has a Name property. <span class='tip tip-lava'></span>", 4, @"", "44836A7C-5007-4213-9AE7-E787225659C3" ); // Rock.Workflow.Action.SetAttributeFromEntity:Lava Template
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1" ); // Rock.Workflow.Action.SetAttributeFromEntity:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Entity Is Required", "EntityIsRequired", "Should an error be returned if the entity is missing or not a valid entity type?", 2, @"True", "05B2AC7A-6C94-41C0-BF92-EE3DEF7895B1" ); // Rock.Workflow.Action.SetAttributeFromEntity:Entity Is Required
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Use Id instead of Guid", "UseId", "Most entity attribute field types expect the Guid of the entity (which is used by default). Select this option if the entity's Id should be used instead (should be rare).", 3, @"False", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B" ); // Rock.Workflow.Action.SetAttributeFromEntity:Use Id instead of Guid
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attribute", "Attribute", "The attribute to set the value of.", 1, @"", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7" ); // Rock.Workflow.Action.SetAttributeFromEntity:Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "AD4EFAC4-E687-43DF-832F-0DC3856ABABB" ); // Rock.Workflow.Action.SetAttributeFromEntity:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "9A4A3446-56D0-44AE-BFC7-22006547F22E" ); // Rock.Workflow.Action.SendSms:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Message|Attribute Value", "Message", "The message or an attribute that contains the message that should be sent. <span class='tip tip-lava'></span>", 2, @"", "42E9674B-7413-41C4-A3F7-5E8265E83EAE" ); // Rock.Workflow.Action.SendSms:Message|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Recipient|Attribute Value", "To", "The phone number or an attribute that contains the person or phone number that message should be sent to. <span class='tip tip-lava'></span>", 1, @"", "2A4DB4D3-8FDD-4E25-B28E-0FB0FE8ABFA9" ); // Rock.Workflow.Action.SendSms:Recipient|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", "59D5A94C-94A0-4630-B80A-BB25697D74C7", "From", "From", "The phone number to send message from", 0, @"", "85946B94-55DB-4219-A04E-EAB1D8C9022E" ); // Rock.Workflow.Action.SendSms:From
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "05A85E0D-4860-4BFB-ACA9-C7B668B89500" ); // Rock.Workflow.Action.SendSms:Order
            RockMigrationHelper.UpdateCategory( "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE", "Messaging", "fa fa-mobile", "", "D1AF4B37-FFFB-4A05-9A7B-7BCB7A866D0D", 0 ); // Messaging
            RockMigrationHelper.UpdateWorkflowType( false, true, "Send A Text Message", "Sends a text message to a user from their profile page.", "D1AF4B37-FFFB-4A05-9A7B-7BCB7A866D0D", "Send A Text", "fa fa-mobile", 0, true, 0, "F3218F81-423F-46EA-B748-ED7AB365CD07" ); // Send A Text Message
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "F3218F81-423F-46EA-B748-ED7AB365CD07", "C28C7BF3-A552-4D77-9408-DEDCF760CED0", "Message", "Message", "The message to send", 2, @"", "D4438B4B-8D7C-49F1-B025-F37BEC10E3D3" ); // Send A Text Message:Message
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "F3218F81-423F-46EA-B748-ED7AB365CD07", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Recipient", "recipient", "Sets the recipient of the message", 0, @"", "494280DA-D7BF-4523-9774-20B477062113" ); // Send A Text Message:Recipient
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "F3218F81-423F-46EA-B748-ED7AB365CD07", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Sender", "Sender", "Sets the sender", 1, @"", "3E27AC12-BE52-4CBA-AABE-F97D58523121" ); // Send A Text Message:Sender
            RockMigrationHelper.AddAttributeQualifier( "D4438B4B-8D7C-49F1-B025-F37BEC10E3D3", "allowhtml", @"False", "5DA52708-4152-416D-812D-969BFB1B6FE0" ); // Send A Text Message:Message:allowhtml
            RockMigrationHelper.AddAttributeQualifier( "D4438B4B-8D7C-49F1-B025-F37BEC10E3D3", "numberofrows", @"", "643FF567-279A-469C-94C2-ABE76BAFDA74" ); // Send A Text Message:Message:numberofrows
            RockMigrationHelper.UpdateWorkflowActivityType( "F3218F81-423F-46EA-B748-ED7AB365CD07", true, "Send The Text", "", false, 0, "9D5C50E2-9D67-4362-B398-7E9E2B488283" ); // Send A Text Message:Send The Text
            RockMigrationHelper.UpdateWorkflowActionForm( @"Enter your message:", @"", "Send^fdc397cd-8b4a-436e-bea1-bce2e6717c03^^Your text has been sent!|", "88C7D1CC-3478-4562-A301-AE7D4D7FFF6D", true, "", "C961B02A-CD18-40F5-B323-DBBA067B4D8A" ); // Send A Text Message:Send The Text:Enter The Message
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "C961B02A-CD18-40F5-B323-DBBA067B4D8A", "494280DA-D7BF-4523-9774-20B477062113", 0, false, true, false, "78723B66-8B8D-4398-8859-8DA30153E54B" ); // Send A Text Message:Send The Text:Enter The Message:Recipient
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "C961B02A-CD18-40F5-B323-DBBA067B4D8A", "3E27AC12-BE52-4CBA-AABE-F97D58523121", 1, false, true, false, "5742591D-5D4A-40AC-91A8-A868DBACE1C9" ); // Send A Text Message:Send The Text:Enter The Message:Sender
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "C961B02A-CD18-40F5-B323-DBBA067B4D8A", "D4438B4B-8D7C-49F1-B025-F37BEC10E3D3", 2, true, false, true, "B72D158D-AA06-4770-BB6C-832A519FC388" ); // Send A Text Message:Send The Text:Enter The Message:Message
            RockMigrationHelper.UpdateWorkflowActionType( "9D5C50E2-9D67-4362-B398-7E9E2B488283", "Sender", 1, "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", true, false, "", "", 1, "", "9ACD91CE-9946-4DB4-8985-8BAB84679862" ); // Send A Text Message:Send The Text:Sender
            RockMigrationHelper.UpdateWorkflowActionType( "9D5C50E2-9D67-4362-B398-7E9E2B488283", "Sets The Recipient", 0, "972F19B9-598B-474B-97A4-50E56E7B59D2", true, false, "", "", 1, "", "B433A848-90B1-467D-8B16-2080A8331965" ); // Send A Text Message:Send The Text:Sets The Recipient
            RockMigrationHelper.UpdateWorkflowActionType( "9D5C50E2-9D67-4362-B398-7E9E2B488283", "Enter The Message", 2, "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", true, false, "C961B02A-CD18-40F5-B323-DBBA067B4D8A", "", 1, "", "3CB6173C-9F2F-4E7F-A998-9B4832C539B0" ); // Send A Text Message:Send The Text:Enter The Message
            RockMigrationHelper.UpdateWorkflowActionType( "9D5C50E2-9D67-4362-B398-7E9E2B488283", "Send Message", 3, "F22FA171-B5E7-497F-9AE6-F7B98A377D0E", true, false, "", "", 1, "", "7EAE659C-5A08-4BFF-869C-A12FA5ED021E" ); // Send A Text Message:Send The Text:Send Message
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1", @"False" ); // Send A Text Message:Send The Text:Sets The Recipient:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "AD4EFAC4-E687-43DF-832F-0DC3856ABABB", @"" ); // Send A Text Message:Send The Text:Sets The Recipient:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7", @"494280da-d7bf-4523-9774-20b477062113" ); // Send A Text Message:Send The Text:Sets The Recipient:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "05B2AC7A-6C94-41C0-BF92-EE3DEF7895B1", @"True" ); // Send A Text Message:Send The Text:Sets The Recipient:Entity Is Required
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B", @"False" ); // Send A Text Message:Send The Text:Sets The Recipient:Use Id instead of Guid
            RockMigrationHelper.AddActionTypeAttributeValue( "B433A848-90B1-467D-8B16-2080A8331965", "44836A7C-5007-4213-9AE7-E787225659C3", @"" ); // Send A Text Message:Send The Text:Sets The Recipient:Lava Template
            RockMigrationHelper.AddActionTypeAttributeValue( "9ACD91CE-9946-4DB4-8985-8BAB84679862", "DE9CB292-4785-4EA3-976D-3826F91E9E98", @"False" ); // Send A Text Message:Send The Text:Sender:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "9ACD91CE-9946-4DB4-8985-8BAB84679862", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112", @"3e27ac12-be52-4cba-aabe-f97d58523121" ); // Send A Text Message:Send The Text:Sender:Person Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "9ACD91CE-9946-4DB4-8985-8BAB84679862", "89E9BCED-91AB-47B0-AD52-D78B0B7CB9E8", @"" ); // Send A Text Message:Send The Text:Sender:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "3CB6173C-9F2F-4E7F-A998-9B4832C539B0", "234910F2-A0DB-4D7D-BAF7-83C880EF30AE", @"False" ); // Send A Text Message:Send The Text:Enter The Message:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "3CB6173C-9F2F-4E7F-A998-9B4832C539B0", "C178113D-7C86-4229-8424-C6D0CF4A7E23", @"" ); // Send A Text Message:Send The Text:Enter The Message:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "7EAE659C-5A08-4BFF-869C-A12FA5ED021E", "85946B94-55DB-4219-A04E-EAB1D8C9022E", @"d4a21848-fd21-4af8-952c-a095fdc177cb" ); // Send A Text Message:Send The Text:Send Message:From
            RockMigrationHelper.AddActionTypeAttributeValue( "7EAE659C-5A08-4BFF-869C-A12FA5ED021E", "05A85E0D-4860-4BFB-ACA9-C7B668B89500", @"" ); // Send A Text Message:Send The Text:Send Message:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "7EAE659C-5A08-4BFF-869C-A12FA5ED021E", "9A4A3446-56D0-44AE-BFC7-22006547F22E", @"False" ); // Send A Text Message:Send The Text:Send Message:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "7EAE659C-5A08-4BFF-869C-A12FA5ED021E", "2A4DB4D3-8FDD-4E25-B28E-0FB0FE8ABFA9", @"494280da-d7bf-4523-9774-20b477062113" ); // Send A Text Message:Send The Text:Send Message:Recipient|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "7EAE659C-5A08-4BFF-869C-A12FA5ED021E", "42E9674B-7413-41C4-A3F7-5E8265E83EAE", @"{{ Workflow.Message }} - {{ Workflow.Sender }} @ NewSpring Church" ); // Send A Text Message:Send The Text:Send Message:Message|Attribute Value
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.DeleteWorkflowType( "F3218F81-423F-46EA-B748-ED7AB365CD07" );
        }
    }
}