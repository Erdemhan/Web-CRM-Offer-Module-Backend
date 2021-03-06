using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;
using crmweb.Models.CompanyContactModels;

namespace crmweb.Models.CompanyModels
{
    public class CompanyInfo
    {
        [Required]
        public int Id { get; set; } //soru işaretini kaldırdın
        [Required]
        [StringLength(50, ErrorMessage = "String Lenght not be long more than 50")]
        public string Prefix { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "String Lenght not be long more than 50")]
        public string Name { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "String Lenght not be long more than 15")]
        public string Phone { get; set; }
        [StringLength(15, ErrorMessage = "String Lenght not be long more than 15")]
        public string Fax { get; set; }
        [StringLength(150, ErrorMessage = "String Lenght not be long more than 150")]
        public string Address { get; set; }

    }
}
