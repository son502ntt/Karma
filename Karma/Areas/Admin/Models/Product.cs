using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public string MaLoai { get; set; }
        public List<Category> TenLoai { get; set; } = new List<Category>();
       
    }
}