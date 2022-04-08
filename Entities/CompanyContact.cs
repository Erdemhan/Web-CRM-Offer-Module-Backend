using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Base;

namespace crmweb.Data.Entities
{
    public class CompanyContact:Entity
    {
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<OfferHeader> OfferHeaders { get; set; }

    }
}
