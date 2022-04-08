using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;

namespace crmweb.Models.OfferModels
{
    public class OfferInfo
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int CompanyContactId { get; set; }
        [Required]
        public byte State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? ValidationDate { get; set; }

        public int OfferNo { get; set; }
        public int RevisionNo { get; set; }

        public string OfferCode { get; set; }

        [StringLength(1000,ErrorMessage = "String Lenght not be long from 1000")]
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
        [Required]
        public int CreateById { get; set; }

        public ICollection<OfferDetailsInfo> OfferDetail { get; set; }

    }
}
