﻿// <copyright>
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
using System.IO;
using System.Web.Http;
using Newtonsoft.Json;

using Rock.Model;
using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// Users REST API
    /// </summary>
    public partial class UserLoginsController : IHasCustomRoutes
    {
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "UsernameAvailable",
                routeTemplate: "api/userlogins/available/{username}",
                defaults: new
                {
                    controller = "userlogins",
                    action = "available"
                } );
        }

        /// <summary>
        /// Tests if a username is available
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Available( string username )
        {
            return ( (UserLoginService)Service ).GetByUserName( username ) == null;
        }

        /// <summary>
        /// Posts the specified value.
        /// POST using <see cref="Rock.Model.UserLoginWithPlainTextPassword"/> and set PlainTextPassword to set a password.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [HttpPost]
        [Authenticate, Secured]
        public override System.Net.Http.HttpResponseMessage Post( UserLogin value )
        {
            SetPasswordFromRest( value );
            return base.Post( value );
        }

        /// <summary>
        /// Puts the specified identifier.
        /// PUT using <see cref="Rock.Model.UserLoginWithPlainTextPassword"/> and set PlainTextPassword to set a password.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        [Authenticate, Secured]
        public override void Put( int id, UserLogin value )
        {
            SetPasswordFromRest( value );
            base.Put( id, value );
        }

        /// <summary>
        /// Sets the password from rest.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetPasswordFromRest( UserLogin value )
        {
            UserLoginWithPlainTextPassword userLoginWithPlainTextPassword = null;
            string json = string.Empty;

            this.ActionContext.Request.Content.ReadAsStreamAsync().ContinueWith( a =>
            {
                // to allow PUT and POST to work with either UserLogin or UserLoginWithPlainTextPassword, read the the stream into a UserLoginWithPlainTextPassword record to see if that's what we got
                a.Result.Position = 0;
                StreamReader sr = new StreamReader( a.Result );

                json = sr.ReadToEnd();
                userLoginWithPlainTextPassword = JsonConvert.DeserializeObject( json, typeof( UserLoginWithPlainTextPassword ) ) as UserLoginWithPlainTextPassword;
            } ).Wait();

            if ( userLoginWithPlainTextPassword != null )
            {
                // if a UserLoginWithPlainTextPassword was posted, and PlainTextPassword was specified, encrypt it and set UserLogin.Password
                if ( !string.IsNullOrWhiteSpace( userLoginWithPlainTextPassword.PlainTextPassword ) )
                {
                    ( this.Service as UserLoginService ).SetPassword( value, userLoginWithPlainTextPassword.PlainTextPassword );
                }
            }
            else
            {
                // since REST doesn't serialize Password, get the existing Password from the database so that it doesn't get NULLed out
                var existingUserLoginRecord = this.Get( value.Id );
                if (existingUserLoginRecord != null)
                {
                    value.Password = existingUserLoginRecord.Password;
                }
            }
        }
    }
}