using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Products
    {
        [Display(Name = "Giá tiền")]
        public double? Gia { get; set; }
        [Display(Name = "Mã loại")]
        public string MaLoai { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string MaSanPham { get; set; }
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; }
        public string TenLoai { get; set; }
        [Display(Name = "Tình trạng")]
        public string TinhTrang { get; set; }
        [Display(Name = "Giá gốc")]
        public double? GiaGoc { get { return Gia + Gia / 2; } }
        [Display(Name = "Ảnh sản phẩm")]
        public string AnhSanPham { get; set; }
    }
}