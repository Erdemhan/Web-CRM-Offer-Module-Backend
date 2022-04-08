using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using crmweb.Data.Base;

namespace crmweb.Data.Entities
{
    public class OfferDetail : Entity
    {
        public int OfferId { get; set; }
        public string Summary { get; set; }
        public byte IsOptional { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        [NotMapped]
        //public IFormFile DetailImage { get; set; }
        public string Description { get; set; }

        // Navigation Properties
        public virtual OfferHeader DetailOfferHeader { get; set; }
    }
}
