using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Serializers;

namespace cc.newspring.Apollos.Utilities
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public NewtonsoftJsonSerializer( ISerializer serializer )
        {
            ContentType = serializer.ContentType;
            DateFormat = serializer.DateFormat;
            Namespace = serializer.Namespace;
            RootElement = serializer.RootElement;
        }

        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize( object obj )
        {
            var json = string.Empty;
            var group = obj as Rock.Model.Group;

            if ( group != null )
            {
                var temp = group.GroupType;
                group.GroupType = null;
                json = Newtonsoft.Json.JsonConvert.SerializeObject( group );
                group.GroupType = temp;
            }
            else
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject( obj );
            }
                        
            return json;
        }
    }
}