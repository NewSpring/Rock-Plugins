using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using cc.newspring.Apollos.Utilities;
using RestSharp;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;

namespace cc.newspring.Apollos.Workflow.Action
{
    [Description( "Sync with API" )]
    [Export( typeof( Rock.Workflow.ActionComponent ) )]
    [ExportMetadata( "ComponentName", "API Sync" )]
    [CustomDropdownListField( "Action", "The workflow that this action is under is triggered by what type of event", "Save,Delete", true )]
    [TextField( "Sync URL", "The specific URL endpoint this related entity type should synchronize with", true, "" )]
    [TextField( "Token Name", "The key by which the token should be identified in the header of HTTP requests", false, "" )]
    [TextField( "Token Value", "The value of the token to authenticate with the URL endpoint", false, "" )]
    public class APISync : Rock.Workflow.ActionComponent
    {
        public override bool Execute( Rock.Data.RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            var isSave = GetAttributeValue( action, "Action" ) == "Save";
            var url = GetAttributeValue( action, "SyncURL" );
            var tokenName = GetAttributeValue( action, "TokenName" );
            var tokenValue = GetAttributeValue( action, "TokenValue" );
            var lastSlash = "/";

            if ( string.IsNullOrWhiteSpace( url ) )
            {
                return true;
            }

            if ( url.EndsWith( lastSlash ) )
            {
                lastSlash = string.Empty;
            }

            var request = new RestRequest( isSave ? Method.POST : Method.DELETE );
            request.RequestFormat = DataFormat.Json;
            request.AddHeader( tokenName, tokenValue );

            var model = (IModel)entity;
            var fullUrl = string.Format( "{0}{1}{2}", url, lastSlash, model.Id );
            var client = new RestClient( fullUrl );

            if ( isSave )
            {
                request.JsonSerializer = new NewtonsoftJsonSerializer( request.JsonSerializer );
                request.AddJsonBody( model );
            }

            var response = client.Execute( request );
            return true;
        }
    }
}