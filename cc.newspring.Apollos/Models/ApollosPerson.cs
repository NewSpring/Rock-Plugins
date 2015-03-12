using System;
using Rock.Model;

namespace cc.newspring.Apollos
{
    public class ApollosPerson
    {
        public ApollosPerson( Person person )
        {
            Guid = person.Guid;
            Id = person.Id;
            Gender = person.Gender;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            NickName = person.NickName;
            Email = person.Email;
            EmailPreference = person.EmailPreference;
            AnniversaryDate = person.AnniversaryDate;
            BirthDay = person.BirthDay;
            BirthMonth = person.BirthMonth;
            BirthYear = person.BirthYear;
        }

        public Guid Guid { get; set; }

        public Gender Gender { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public EmailPreference EmailPreference { get; set; }

        public DateTime? AnniversaryDate { get; set; }

        public int? BirthDay { get; set; }

        public int? BirthMonth { get; set; }

        public int? BirthYear { get; set; }
    }
}