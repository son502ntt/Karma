using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Models
{
    public class Product
    {
        [Display(Name = "Mã sản phẩm")]
        public string MaSanPham { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; }
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
        [Display(Name = "Tình trạng")]
        public string TinhTrang { get; set; }
        [Display(Name = "Giá tiền")]
        public string Gia { get; set; }
        [Display(Name = "Ảnh sản phẩm")]
        public string AnhSanPham { get; set; }
        [Display(Name = "Số lượng")]
        public string SoLuong { get; set; }
        [Display(Name = "Tên loại")]
        public string MaLoai { get; set; }
        [Display(Name = "Tên loại")]
        public List<Category> ListLoai { get; set; } = new List<Category>();
        [Display(Name = "Tên loại")]
        public string TenLoai { get; set; }
    }
}