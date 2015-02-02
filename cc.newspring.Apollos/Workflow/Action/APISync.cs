using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;

namespace cc.newspring.Apollos.Workflow.Action
{
    [Description( "Sync with API" )]
    [Export( typeof( Rock.Workflow.ActionComponent ) )]
    [ExportMetadata( "ComponentName", "API Sync" )]
    [TextField( "Sync URL", "The specific URL endpoint this related entity type should synchronize with", false, "" )]
    [TextField( "Token Name", "The key by which the token should be identified in the header of HTTP requests", false, "" )]
    [TextField( "Token Value", "The value of the token to authenticate with the URL endpoint", false, "" )]
    public class APISync : Rock.Workflow.ActionComponent
    {
        public override bool Execute( Rock.Data.RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            return true;
        }
    }
}