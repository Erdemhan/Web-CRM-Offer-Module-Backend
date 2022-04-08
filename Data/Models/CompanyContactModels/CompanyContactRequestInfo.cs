using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;

namespace crmweb.Models.CompanyContactModels
{
    public class CompanyContactRequestInfo
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
