using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.cc_newspring.Video
{
    [TextField( "Ooyala Content ID", "Paste the Ooyala Content ID here" )]
    public partial class Ooyala : Rock.Web.UI.RockBlock
    {
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // Set the ooyala id
            ooyalaId.Value = GetAttributeValue( "OoyalaContentID" );
        }
    }
}