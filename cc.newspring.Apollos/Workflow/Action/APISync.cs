using System;
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
    [Description( "Sync to a third party API" )]
    [Export( typeof( Rock.Workflow.ActionComponent ) )]
    [ExportMetadata( "ComponentName", "API Sync" )]
    [CustomDropdownListField( "Action", "The workflow that this action is under is triggered by what type of event", "Save,Delete", true )]
    [TextField( "Sync URL", "The specific URL endpoint this related entity type should synchronize with", true, "" )]
    [TextField( "Token Name", "The key by which the token should be identified in the header of HTTP requests", false, "" )]
    [TextField( "Token Value", "The value of the token to authenticate with the URL endpoint", false, "" )]
    [PersonField( "Rest User", "The associated REST user that handles sync from the third party", false, "" )]
    public class APISync : Rock.Workflow.ActionComponent
    {
        public override bool Execute( Rock.Data.RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();
            var castedModel = entity as IModel;

            if ( castedModel != null )
            {
                var restUserGuid = GetAttributeValue( action, "RestUser" ).AsType<Guid?>();

                if ( castedModel.ModifiedByPersonAliasId.HasValue )
                {
                    var modifier = new PersonAliasService( new RockContext() ).Get( castedModel.ModifiedByPersonAliasId.Value );

                    if ( modifier.Guid == restUserGuid )
                    {
                        // If the modifier is Apollos, don't send the data to Apollos (bounceback sync)
                        return true;
                    }
                }
                else if ( castedModel.CreatedByPersonAliasId.HasValue )
                {
                    var modifier = new PersonAliasService( new RockContext() ).Get( castedModel.CreatedByPersonAliasId.Value );

                    if ( modifier.Guid == restUserGuid )
                    {
                        return true;
                    }
                }
            }

            var castedEntity = entity as IEntity;
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

            var fullUrl = string.Format( "{0}{1}{2}", url, lastSlash, castedEntity.Id );
            var client = new RestClient( fullUrl );

            if ( isSave )
            {
                request.JsonSerializer = new NewtonsoftJsonSerializer( request.JsonSerializer );

                if ( !( entity is UserLogin ) )
                {
                    request.AddJsonBody( entity );
                }
                else
                {
                    // Do this so the password hash is synced
                    var apollosUser = new ApollosUserLogin( (UserLogin)entity );
                    request.AddJsonBody( apollosUser );
                }
            }

            var response = client.Execute( request );
            return true;
        }
    }
}