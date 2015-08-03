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
    [MigrationNumber( 25, "1.3.0" )]
    public class CareRequestFollowUp : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateEntityType( "Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.CompleteWorkflow", "EEDA4318-F014-4A46-9C76-4C052EF81AA1", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.PersistWorkflow", "F1A39347-6FE0-43D4-89FB-544195088ECF", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.PersonNoteAdd", "C485BF81-1632-4211-A67C-CBB036B14DF9", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeToCurrentPerson", "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetPersonAttribute", "320622DA-52E0-41AE-AF90-2BF78B488552", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetWorkflowName", "36005473-BD5D-470B-B28D-98E6D7ED808D", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.UserEntryForm", "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", false, true );
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "DE9CB292-4785-4EA3-976D-3826F91E9E98" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Person Attribute", "PersonAttribute", "The attribute to set to the currently logged in person.", 0, @"", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Person Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "89E9BCED-91AB-47B0-AD52-D78B0B7CB9E8" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "320622DA-52E0-41AE-AF90-2BF78B488552", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "E5BAC4A6-FF7F-4016-BA9C-72D16CB60184" ); // Rock.Workflow.Action.SetPersonAttribute:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "320622DA-52E0-41AE-AF90-2BF78B488552", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Person", "Person", "Workflow attribute that contains the person to update.", 0, @"", "E456FB6F-05DB-4826-A612-5B704BC4EA13" ); // Rock.Workflow.Action.SetPersonAttribute:Person
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "320622DA-52E0-41AE-AF90-2BF78B488552", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Value|Attribute Value", "Value", "The value or attribute value to set the person attribute to. <span class='tip tip-lava'></span>", 2, @"", "94689BDE-493E-4869-A614-2D54822D747C" ); // Rock.Workflow.Action.SetPersonAttribute:Value|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "320622DA-52E0-41AE-AF90-2BF78B488552", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Person Attribute", "PersonAttribute", "The person attribute that should be updated with the provided value.", 1, @"", "8F4BB00F-7FA2-41AD-8E90-81F4DFE2C762" ); // Rock.Workflow.Action.SetPersonAttribute:Person Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "320622DA-52E0-41AE-AF90-2BF78B488552", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "3F3BF3E6-AD53-491E-A40F-441F2AFCBB5B" ); // Rock.Workflow.Action.SetPersonAttribute:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "36005473-BD5D-470B-B28D-98E6D7ED808D", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "0A800013-51F7-4902-885A-5BE215D67D3D" ); // Rock.Workflow.Action.SetWorkflowName:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "36005473-BD5D-470B-B28D-98E6D7ED808D", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Text Value|Attribute Value", "NameValue", "The value to use for the workflow''s name. <span class='tip tip-lava'></span>", 1, @"", "93852244-A667-4749-961A-D47F88675BE4" ); // Rock.Workflow.Action.SetWorkflowName:Text Value|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "36005473-BD5D-470B-B28D-98E6D7ED808D", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "5D95C15A-CCAE-40AD-A9DD-F929DA587115" ); // Rock.Workflow.Action.SetWorkflowName:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "234910F2-A0DB-4D7D-BAF7-83C880EF30AE" ); // Rock.Workflow.Action.UserEntryForm:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "C178113D-7C86-4229-8424-C6D0CF4A7E23" ); // Rock.Workflow.Action.UserEntryForm:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "8AF1D528-3F5F-4C3C-A65B-362B577350E2" ); // Rock.Workflow.Action.PersonNoteAdd:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Alert", "Alert", "Determines if the note should be flagged as an alert.", 5, @"False", "3557B6D2-6AF1-4511-95FB-A3D732435CA3" ); // Rock.Workflow.Action.PersonNoteAdd:Alert
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Author", "Author", "Workflow attribute that contains the person to use as the author of the note. While not required it is recommended.", 4, @"", "5B0BE9C1-0CEF-40BC-A024-C9A8BA278F2B" ); // Rock.Workflow.Action.PersonNoteAdd:Author
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Person", "Person", "Workflow attribute that contains the person to add the note to.", 0, @"", "8A58B31E-B94B-448B-A1C5-B8AB1391AC93" ); // Rock.Workflow.Action.PersonNoteAdd:Person
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Caption", "Caption", "The title/caption of the note. If none is provided then the author''s name will be displayed. <span class='tip tip-lava'></span>", 2, @"", "6346FA38-6415-44A3-B8B4-8946A40CB144" ); // Rock.Workflow.Action.PersonNoteAdd:Caption
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "05FBA26D-C3C6-41A3-81CF-C294AA574722" ); // Rock.Workflow.Action.PersonNoteAdd:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "C28C7BF3-A552-4D77-9408-DEDCF760CED0", "Text", "Text", "The body of the note. <span class='tip tip-lava'></span>", 3, @"", "F63E9C66-C061-41FA-A772-0B1353A2F45E" ); // Rock.Workflow.Action.PersonNoteAdd:Text
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C485BF81-1632-4211-A67C-CBB036B14DF9", "E3FF88AC-13F6-4DF8-8371-FC0D7FD9A571", "Note Type", "NoteType", "The type of note to add.", 1, @"66A1B9D7-7EFA-40F3-9415-E54437977D60", "FE4156ED-EC3E-49DB-83B4-88F164B3C166" ); // Rock.Workflow.Action.PersonNoteAdd:Note Type
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "EEDA4318-F014-4A46-9C76-4C052EF81AA1", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "0CA0DDEF-48EF-4ABC-9822-A05E225DE26C" ); // Rock.Workflow.Action.CompleteWorkflow:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "EEDA4318-F014-4A46-9C76-4C052EF81AA1", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "25CAD4BE-5A00-409D-9BAB-E32518D89956" ); // Rock.Workflow.Action.CompleteWorkflow:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "50B01639-4938-40D2-A791-AA0EB4F86847" ); // Rock.Workflow.Action.PersistWorkflow:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Persist Immediately", "PersistImmediately", "This action will normally cause the workflow to be persisted (saved) once all the current activites/actions have completed processing. Set this flag to true, if the workflow should be persisted immediately. This is only required if a subsequent action needs a persisted workflow with a valid id.", 0, @"False", "CB7BF538-99A2-4BAE-B848-D96269FB541C" ); // Rock.Workflow.Action.PersistWorkflow:Persist Immediately
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "86F795B0-0CB6-4DA4-9CE4-B11D0922F361" ); // Rock.Workflow.Action.PersistWorkflow:Order
            RockMigrationHelper.UpdateWorkflowType( false, true, "Care Follow Up", "Used on public website when Care volunteer has made contact with a guest during the week.", "14947674-A075-4AC7-BF99-79326980ED9D", "Inquiry", "fa fa-comments-o", 0, false, 3, "D96FF580-362E-461F-9952-7EC885563C09" ); // Care Follow Up
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "1B71FEF4-201F-4D53-8C60-2DF21F1985ED", "Campus", "Campus", "The campus where the initial Care conversation happened.", 2, @"", "4EDFE1EF-B010-4CD8-AC83-D7957E62EB9D" ); // Care Follow Up:Campus
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Were you able to connect with the Guest?", "ConnectStatus", "", 3, @"", "9D7AC979-47C0-422D-AB92-C38F64C4671F" ); // Care Follow Up:Were you able to connect with the Guest?
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "6B6AA175-4758-453F-8D83-FCD8044B5F36", "Date of Follow Up", "DateofFollowUp", "", 4, @"", "40718F75-9A58-41DB-A70F-E2A20862102B" ); // Care Follow Up:Date of Follow Up
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Guest", "Guest", "Notes are automatically saved to this person''s profile so please make sure that you have selected the correct profile before continuing.", 0, @"", "2EC3D684-59DC-4899-8EFE-4E4CF9BE2480" ); // Care Follow Up:Guest
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Hosting Volunteer", "HostingVolunteer", "", 5, @"", "A624E0D0-1513-4E2A-9625-DBA0F9265BB6" ); // Care Follow Up:Hosting Volunteer
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "D96FF580-362E-461F-9952-7EC885563C09", "C28C7BF3-A552-4D77-9408-DEDCF760CED0", "Notes", "Notes", "Any information about your conversation that may be useful for future reference.", 1, @"", "57BA2E8E-7E88-4C0E-A1E5-97704417013E" ); // Care Follow Up:Notes
            RockMigrationHelper.AddAttributeQualifier( "9D7AC979-47C0-422D-AB92-C38F64C4671F", "falsetext", @"Attempted to connect but was unable.", "25A13DE9-7BD2-46C9-BE11-A8DC277604A7" ); // Care Follow Up:Were you able to connect with the Guest?:falsetext
            RockMigrationHelper.AddAttributeQualifier( "9D7AC979-47C0-422D-AB92-C38F64C4671F", "truetext", @"Able to connect with Guest", "2BD707DA-42FC-44A4-A894-44D710C3AF00" ); // Care Follow Up:Were you able to connect with the Guest?:truetext
            RockMigrationHelper.AddAttributeQualifier( "40718F75-9A58-41DB-A70F-E2A20862102B", "displayCurrentOption", @"False", "F82A2A94-21FF-4138-B876-52B6C0D94C86" ); // Care Follow Up:Date of Follow Up:displayCurrentOption
            RockMigrationHelper.AddAttributeQualifier( "40718F75-9A58-41DB-A70F-E2A20862102B", "displayDiff", @"False", "7285C5F5-5D2D-4F24-ACB9-4C7D1F68ECF5" ); // Care Follow Up:Date of Follow Up:displayDiff
            RockMigrationHelper.AddAttributeQualifier( "40718F75-9A58-41DB-A70F-E2A20862102B", "format", @"", "C97E3908-CC86-4490-8648-005150E36DE6" ); // Care Follow Up:Date of Follow Up:format
            RockMigrationHelper.AddAttributeQualifier( "57BA2E8E-7E88-4C0E-A1E5-97704417013E", "allowhtml", @"False", "6B649FA2-4197-46D2-8B1F-5868575CE0C8" ); // Care Follow Up:Notes:allowhtml
            RockMigrationHelper.AddAttributeQualifier( "57BA2E8E-7E88-4C0E-A1E5-97704417013E", "numberofrows", @"", "C58619D1-7392-44BB-A308-14EABC53BA8B" ); // Care Follow Up:Notes:numberofrows
            RockMigrationHelper.UpdateWorkflowActivityType( "D96FF580-362E-461F-9952-7EC885563C09", true, "Request", "Prompt the user for the information about their request", true, 0, "918DDD3C-0A14-42FE-97D3-05DE48E2D670" ); // Care Follow Up:Request
            RockMigrationHelper.UpdateWorkflowActionForm( @"<h2>Care Follow Up Form</h2> <p> Thank you for how well you love on the guests that come into the Care Room! Please complete the form below. </p> <br/>", @"", "Submit^fdc397cd-8b4a-436e-bea1-bce2e6717c03^^Thank you! This information has been recorded successfully.|", "", true, "", "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF" ); // Care Follow Up:Request:Prompt User
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "4EDFE1EF-B010-4CD8-AC83-D7957E62EB9D", 2, true, false, true, "83D7DE1C-E1CD-4F6A-B90B-561321B78819" ); // Care Follow Up:Request:Prompt User:Campus
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "57BA2E8E-7E88-4C0E-A1E5-97704417013E", 5, true, false, false, "9AA35A81-37A0-469A-AF5F-6EEEA7B32D08" ); // Care Follow Up:Request:Prompt User:Notes
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "9D7AC979-47C0-422D-AB92-C38F64C4671F", 3, true, false, true, "711CBABD-B059-4B17-9908-3F3432AE8726" ); // Care Follow Up:Request:Prompt User:Were you able to connect with the Guest?
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "2EC3D684-59DC-4899-8EFE-4E4CF9BE2480", 4, true, false, true, "9F325066-055E-481E-9B37-7B5890C9D05D" ); // Care Follow Up:Request:Prompt User:Guest
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "40718F75-9A58-41DB-A70F-E2A20862102B", 1, true, false, true, "75A00CF3-E424-499E-8AF5-88C05C7C53CD" ); // Care Follow Up:Request:Prompt User:Date of Follow Up
            RockMigrationHelper.UpdateWorkflowActionFormAttribute( "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "A624E0D0-1513-4E2A-9625-DBA0F9265BB6", 0, true, true, false, "703AA987-E3E0-4AD6-80DF-A3E844BFD372" ); // Care Follow Up:Request:Prompt User:Hosting Volunteer
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Prompt User", 1, "486DC4FA-FCBC-425F-90B0-E606DA8A9F68", true, false, "308F6BD3-2D22-4FDB-93D3-522FB8ADDFBF", "", 1, "", "4B002F8A-8197-4001-BEE1-91E05D9FD732" ); // Care Follow Up:Request:Prompt User
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Set Name", 2, "36005473-BD5D-470B-B28D-98E6D7ED808D", true, false, "", "", 1, "", "6EAF8FE8-C5C4-4892-B418-2401FAF667CA" ); // Care Follow Up:Request:Set Name
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Set Volunteer", 0, "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", true, false, "", "", 1, "", "CCDC73A4-D418-4441-9185-B545E5FB5CEB" ); // Care Follow Up:Request:Set Volunteer
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Persist the Workflow", 3, "F1A39347-6FE0-43D4-89FB-544195088ECF", true, false, "", "", 1, "", "88E10712-43B7-415E-94F7-8CD5BF983AB7" ); // Care Follow Up:Request:Persist the Workflow
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Record Notes", 4, "C485BF81-1632-4211-A67C-CBB036B14DF9", true, false, "", "", 64, "", "989288A6-14DF-480D-A610-4F7387DA9EE5" ); // Care Follow Up:Request:Record Notes
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Complete the Workflow", 6, "EEDA4318-F014-4A46-9C76-4C052EF81AA1", true, false, "", "", 1, "", "053CEBF4-6911-4199-97E4-862DD6D5B52E" ); // Care Follow Up:Request:Complete the Workflow
            RockMigrationHelper.UpdateWorkflowActionType( "918DDD3C-0A14-42FE-97D3-05DE48E2D670", "Set Care Follow Up Date Attribute", 5, "320622DA-52E0-41AE-AF90-2BF78B488552", true, false, "", "", 1, "", "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "CCDC73A4-D418-4441-9185-B545E5FB5CEB", "DE9CB292-4785-4EA3-976D-3826F91E9E98", @"False" ); // Care Follow Up:Request:Set Volunteer:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "CCDC73A4-D418-4441-9185-B545E5FB5CEB", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112", @"a624e0d0-1513-4e2a-9625-dba0f9265bb6" ); // Care Follow Up:Request:Set Volunteer:Person Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "CCDC73A4-D418-4441-9185-B545E5FB5CEB", "89E9BCED-91AB-47B0-AD52-D78B0B7CB9E8", @"" ); // Care Follow Up:Request:Set Volunteer:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "4B002F8A-8197-4001-BEE1-91E05D9FD732", "234910F2-A0DB-4D7D-BAF7-83C880EF30AE", @"False" ); // Care Follow Up:Request:Prompt User:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "4B002F8A-8197-4001-BEE1-91E05D9FD732", "C178113D-7C86-4229-8424-C6D0CF4A7E23", @"" ); // Care Follow Up:Request:Prompt User:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "6EAF8FE8-C5C4-4892-B418-2401FAF667CA", "0A800013-51F7-4902-885A-5BE215D67D3D", @"False" ); // Care Follow Up:Request:Set Name:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "6EAF8FE8-C5C4-4892-B418-2401FAF667CA", "5D95C15A-CCAE-40AD-A9DD-F929DA587115", @"" ); // Care Follow Up:Request:Set Name:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "6EAF8FE8-C5C4-4892-B418-2401FAF667CA", "93852244-A667-4749-961A-D47F88675BE4", @"Care Follow Up | {{ Workflow.HostingVolunteer }}" ); // Care Follow Up:Request:Set Name:Text Value|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "88E10712-43B7-415E-94F7-8CD5BF983AB7", "50B01639-4938-40D2-A791-AA0EB4F86847", @"False" ); // Care Follow Up:Request:Persist the Workflow:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "88E10712-43B7-415E-94F7-8CD5BF983AB7", "86F795B0-0CB6-4DA4-9CE4-B11D0922F361", @"" ); // Care Follow Up:Request:Persist the Workflow:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "88E10712-43B7-415E-94F7-8CD5BF983AB7", "CB7BF538-99A2-4BAE-B848-D96269FB541C", @"False" ); // Care Follow Up:Request:Persist the Workflow:Persist Immediately
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "8A58B31E-B94B-448B-A1C5-B8AB1391AC93", @"2ec3d684-59dc-4899-8efe-4e4cf9be2480" ); // Care Follow Up:Request:Record Notes:Person
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "05FBA26D-C3C6-41A3-81CF-C294AA574722", @"" ); // Care Follow Up:Request:Record Notes:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "8AF1D528-3F5F-4C3C-A65B-362B577350E2", @"False" ); // Care Follow Up:Request:Record Notes:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "FE4156ED-EC3E-49DB-83B4-88F164B3C166", @"624D6A68-D20C-4FFE-83DF-630945888A8E" ); // Care Follow Up:Request:Record Notes:Note Type
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "6346FA38-6415-44A3-B8B4-8946A40CB144", @"Care Follow Up Note" ); // Care Follow Up:Request:Record Notes:Caption
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "F63E9C66-C061-41FA-A772-0B1353A2F45E", @"Hosting Volunteer: {{ Workflow.Requester }} {{ Workflow.ConnectStatus }} Notes: {{ Workflow.Notes }} Campus: {{ Workflow.Campus }}" ); // Care Follow Up:Request:Record Notes:Text
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "5B0BE9C1-0CEF-40BC-A024-C9A8BA278F2B", @"" ); // Care Follow Up:Request:Record Notes:Author
            RockMigrationHelper.AddActionTypeAttributeValue( "989288A6-14DF-480D-A610-4F7387DA9EE5", "3557B6D2-6AF1-4511-95FB-A3D732435CA3", @"False" ); // Care Follow Up:Request:Record Notes:Alert
            RockMigrationHelper.AddActionTypeAttributeValue( "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF", "E5BAC4A6-FF7F-4016-BA9C-72D16CB60184", @"False" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF", "E456FB6F-05DB-4826-A612-5B704BC4EA13", @"2ec3d684-59dc-4899-8efe-4e4cf9be2480" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute:Person
            RockMigrationHelper.AddActionTypeAttributeValue( "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF", "3F3BF3E6-AD53-491E-A40F-441F2AFCBB5B", @"" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute:Order
            RockMigrationHelper.AddActionTypeAttributeValue( "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF", "8F4BB00F-7FA2-41AD-8E90-81F4DFE2C762", @"e45c5ffd-31da-42ff-b76c-95b9a99fc03a" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute:Person Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "53D0FC39-ECD2-4F6F-8B3B-28ECA3E99AFF", "94689BDE-493E-4869-A614-2D54822D747C", @"40718f75-9a58-41db-a70f-e2a20862102b" ); // Care Follow Up:Request:Set Care Follow Up Date Attribute:Value|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "053CEBF4-6911-4199-97E4-862DD6D5B52E", "0CA0DDEF-48EF-4ABC-9822-A05E225DE26C", @"False" ); // Care Follow Up:Request:Complete the Workflow:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "053CEBF4-6911-4199-97E4-862DD6D5B52E", "25CAD4BE-5A00-409D-9BAB-E32518D89956", @"" ); // Care Follow Up:Request:Complete the Workflow:Order
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.DeleteWorkflowType( "3969F564-1E30-4851-AF11-E70283BA9DD7" );
        }
    }
}