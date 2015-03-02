using System.Collections;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rock.Data;
using Rock.Model;
using Rock.Rest;
using Rock.Rest.Controllers;
using Rock.Rest.Filters;

namespace cc.newspring.Apollos.Rest.Controllers
{
    public class ApollosUserLoginsController : UserLoginsController
    {
        /// <summary>
        /// Returns a single ApollosUserLogin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/UserLogins/{id}" )]
        public new ApollosUserLogin GetById( int id )
        {
            return new ApollosUserLogin( base.GetById( id ) );
        }

        /// <summary>
        /// Returns all ApollosUserLogins
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/UserLogins" )]
        public new object[] Get()
        {
            var logins = base.Get();
            var apollosLogins = new Queue();

            foreach ( var login in logins )
            {
                apollosLogins.Enqueue( new ApollosUserLogin( login ) );
            }

            return apollosLogins.ToArray();
        }
    }
}