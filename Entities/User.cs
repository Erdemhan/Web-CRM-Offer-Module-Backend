using System.Collections.Generic;
using crmweb.Data.Base;

namespace crmweb.Data.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte Role { get; set; }
        public bool IsActive { get; set; }


        // Navigation Properties
        public virtual ICollection<OfferHeader> CreatedOffers { get; set; }
    }
}
