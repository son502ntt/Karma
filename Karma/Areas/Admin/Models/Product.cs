using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.Models
{
    public class Product
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public string TinhTrang { get; set; }
        public string Gia { get; set; }
        public string AnhSanPham { get; set; }
        public string SoLuong { get; set; }
        public List<string> TenLoai { get; set; }
    }
}