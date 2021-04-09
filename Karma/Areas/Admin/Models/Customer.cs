using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.Models
{
    public class Customer
    {
        public string ID { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaCHi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string Avatar { get; set; }
    }
}