using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.Models
{
    public class Login
    {
        [Key]
        public int id { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}