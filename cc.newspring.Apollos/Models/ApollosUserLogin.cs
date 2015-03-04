using System;
using Rock.Model;

namespace cc.newspring.Apollos
{
    public class ApollosUserLogin
    {
        public ApollosUserLogin( UserLogin userLogin )
        {
            Guid = userLogin.Guid;
            Id = userLogin.Id;
            PersonId = userLogin.PersonId;
            ApollosHash = userLogin.Password;
            UserName = userLogin.UserName;
        }

        public Guid? Guid { get; set; }

        public int? Id { get; set; }

        public int? PersonId { get; set; }

        public string ApollosHash { get; set; }

        public string UserName { get; set; }
    }
}