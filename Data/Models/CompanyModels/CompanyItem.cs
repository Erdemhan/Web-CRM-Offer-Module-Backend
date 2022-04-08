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
    public class CompanyItem
    {
        [Required]
        public int Id { get; set; } 
        public string Name { get; set; }
    }
}
