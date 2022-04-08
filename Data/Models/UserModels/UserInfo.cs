using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace crmweb.Models.UserModels
{
    public class UserInfo
    {
        [Required]
        public int Id{ get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public byte Role { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
