using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace crmweb.Models.OfferModels
{
    public class OfferRequestInfo
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int CompanyContactId { get; set; }
        [Required]
        public int CreateById { get; set; }
        [StringLength(1000, ErrorMessage = "String Lenght not be long from 1000")]
        public string Header { get; set; }
        [StringLength(5000, ErrorMessage = "String Lenght not be long from 5000")]
        public string Description { get; set; }
        [StringLength(4, ErrorMessage = "String Lenght not be long from 4")]
        public string Currency { get; set; }
        [StringLength(3000, ErrorMessage = "String Lenght not be long from 3000")]
        public string CommercialConditions { get; set; }
        [StringLength(3000, ErrorMessage = "String Lenght not be long from 3000")]
        public string Payment { get; set; }
        [StringLength(5000, ErrorMessage = "String Lenght not be long from 5000")]
        public string Times { get; set; }
        [StringLength(5000, ErrorMessage = "String Lenght not be long from 5000")]
        public string AdditionalDescriptions { get; set; }
        public  List<OfferDetailsRequestInfo> OfferDetail { get; set; }
    }
}
