/* Apollos Authentication provider developed using BCrypt.Net:
     * http://bcrypt.codeplex.com/
     * Copyright (c) 2006, 2010, Damien Miller <djm@mindrot.org>, Ryan Emerle
     *
     * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
     * documentation and/or other materials provided with the distribution.
     * Neither the name of BCrypt.Net nor the names of its contributors may be used to endorse or promote products derived from this software without
     * specific prior written permission.
     *
     * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
     * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
     * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
     * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
     * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
     * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
     */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Apollos;
using BCrypt;
using BCrypt.Net;
using RestSharp;
using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Net;
using Rock.Security;

namespace cc.newspring.Apollos
{
    [Description( "Sync with Apollos" )]
    [Export( typeof( Rock.Workflow.ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Apollos Sync" )]
    [TextField( "Sync URL", "The URL endpoint to synchronize with", false, "" )]
    [TextField( "Token Name", "The key by which the token should be identified in the header of HTTP requests", false, "" )]
    [TextField( "Token Value", "The value of the token to authenticate with the URL endpoint", false, "" )]
    internal class ApollosSync : Rock.Workflow.ActionComponent
    {
        public override bool Execute( Rock.Data.RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            return true;
        }
    }
}