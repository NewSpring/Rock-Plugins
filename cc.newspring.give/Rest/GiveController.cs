using System.Linq;
using System.Net;
using System.Web.Http;

using cc.newspring.give.Model;

using Rock.Rest;
using Rock.Rest.Filters;

namespace cc.newspring.give.Rest
{
    /// <summary>
    ///
    /// </summary>
    public class BooksController : ApiController<Book>
    {
        public BooksController() : base( new BookService( new Data.GiveContext() ) ) { }
    }
}
