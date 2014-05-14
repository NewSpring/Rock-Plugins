using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;

using cc.newspring;

using Rock.Data;
using Rock.Model;

namespace cc.newspring.give.Model
{
    /// <summary>
    /// An book
    /// </summary>
    [Table( "_cc_newspring_give_Book" )]
    [DataContract]
    public class Book : cc.newspring.Model<Book>
    {
    }

    #region Entity Configuration

    /// <summary>
    ///
    /// </summary>
    public partial class BookConfiguration : EntityTypeConfiguration<Book>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookConfiguration"/> class.
        /// </summary>
        public BookConfiguration()
        {
        }
    }

    #endregion
}
