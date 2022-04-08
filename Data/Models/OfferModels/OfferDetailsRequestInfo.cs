using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace crmweb.Models.OfferModels
{
    public class OfferDetailsRequestInfo
    {
        public byte IsOptional { get; set; }
        [StringLength(2000, ErrorMessage = "String Lenght not be long from 2000")]
        public string Summary { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        //public IFormFile DetailImage { get; set; }
        [StringLength(5000, ErrorMessage = "String Lenght not be long from 5000")]
        public string Description { get; set; }
    }
}
