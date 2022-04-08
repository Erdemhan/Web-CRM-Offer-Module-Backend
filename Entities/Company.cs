using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Base;

namespace crmweb.Data.Entities
{
    public class Company:Entity
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        [NotMapped]
        //public IFormFile Logo { get; set; }

        public virtual ICollection<CompanyContact> CompanyContact { get; set; }
        public virtual ICollection<OfferHeader> OfferHeaders { get; set; }
    }
}
