using cc.newspring.give.Data;
using Rock.Model;

namespace cc.newspring.give.Model
{
    /// <summary>
    ///
    /// </summary>
    public class BookService : SampleProjectService<Book>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BookService( GiveContext context ) : base( context ) { }
    }
}
