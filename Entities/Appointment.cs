using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Base;

namespace CRMWeb.Entities
{
    public class Appointment : Entity
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set;}

    }
}
