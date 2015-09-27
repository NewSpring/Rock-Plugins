using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.newspring.Apollos
{
    public class PersonParameters
    {
        public string Email { get; set; }

        public int? UserId { get; set; }

        public Guid? PersonGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? CampusId { get; set; }

        public string PhoneNumber { get; set; }
    }
}
