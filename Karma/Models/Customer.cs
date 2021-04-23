using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Customer
    {
        public string ID { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SDT { get; set; }
        public string Avatar { get; set; }
        public string CodeActive { get; set; }
    }
}