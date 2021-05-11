using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class BillDetail
    {
        public string MaDH { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public double? ThanhTien { get; set; }
    }
}