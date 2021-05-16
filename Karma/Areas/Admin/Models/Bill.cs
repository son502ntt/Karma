using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.Models
{
    public class Bill
    {
        public string MaDH { get; set; }
        public string IdKH { get; set; }
        public DateTime NgayGD { get; set; }
        public double TongTien { get; set; }
        public int TongSoLuong { get; set; }
    }
}