using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Login
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}