//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Rock.Models;

namespace Rock.Models.Cms
{
    [Table( "cmsUser" )]
    public partial class User : ModelWithAttributes, IAuditable
    {
		[DataMember]
		public Guid Guid { get; set; }
		
		[MaxLength( 255 )]
		[DataMember]
		public string Username { get; set; }
		
		[MaxLength( 255 )]
		[DataMember]
		public string ApplicationName { get; set; }
		
		[MaxLength( 128 )]
		[DataMember]
		public string Email { get; set; }
		
		[MaxLength( 255 )]
		[DataMember]
		public string Comment { get; set; }
		
		[DataMember]
		public int AuthenticationType { get; set; }
		
		[MaxLength( 128 )]
		[DataMember]
		public string Password { get; set; }
		
		[MaxLength( 255 )]
		[DataMember]
		public string PasswordQuestion { get; set; }
		
		[MaxLength( 255 )]
		[DataMember]
		public string PasswordAnswer { get; set; }
		
		[DataMember]
		public bool? IsApproved { get; set; }
		
		[DataMember]
		public DateTime? LastActivityDate { get; set; }
		
		[DataMember]
		public DateTime? LastLoginDate { get; set; }
		
		[DataMember]
		public DateTime? LastPasswordChangedDate { get; set; }
		
		[DataMember]
		public DateTime? CreationDate { get; set; }
		
		[DataMember]
		public bool? IsOnLine { get; set; }
		
		[DataMember]
		public bool? IsLockedOut { get; set; }
		
		[DataMember]
		public DateTime? LastLockedOutDate { get; set; }
		
		[DataMember]
		public int? FailedPasswordAttemptCount { get; set; }
		
		[DataMember]
		public DateTime? FailedPasswordAttemptWindowStart { get; set; }
		
		[DataMember]
		public int? FailedPasswordAnswerAttemptCount { get; set; }
		
		[DataMember]
		public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
		
		[DataMember]
		public bool? IsSubscriber { get; set; }
		
		[DataMember]
		public int? PersonId { get; set; }
		
		[DataMember]
		public DateTime? CreatedDateTime { get; set; }
		
		[DataMember]
		public DateTime? ModifiedDateTime { get; set; }
		
		[DataMember]
		public int? CreatedByPersonId { get; set; }
		
		[DataMember]
		public int? ModifiedByPersonId { get; set; }
		
		[NotMapped]
		public override string AuthEntity { get { return "Cms.User"; } }

		public virtual ICollection<Role> Roles { get; set; }

		public virtual Crm.Person Person { get; set; }

		public virtual Crm.Person CreatedByPerson { get; set; }

		public virtual Crm.Person ModifiedByPerson { get; set; }
    }

    public partial class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
			this.HasOptional( p => p.Person ).WithMany( p => p.Users ).HasForeignKey( p => p.PersonId );
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId );
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId );
		}
    }
}
