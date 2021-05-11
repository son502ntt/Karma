using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Cart
    {
        [Display(Name = "Mã sản phẩm")]
        public string MaSanPham { get; set; }
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
        [Display(Name = "Đơn giá")]
        public double? DonGia { get; set; }
        [Display(Name = "Thành tiền")]
        public double? ThanhTien { get; set; }
        [Display(Name = "Tổng tiền")]
        public double? TongTien { get; set; }
        [Display(Name = "Tổng số lượng")]
        public int TongSoLuong { get; set; }
        [Display(Name = "Ảnh sản phẩm")]
        public string AnhSanPham { get; set; }

        public  Cart(string MaSP, double Gia, int SL, string TenSP, string AnhSP)
        {
            MaSanPham = MaSP;
            TenSanPham = TenSP;
            AnhSanPham = AnhSP;
            SoLuong = SL;
            DonGia = Gia;
            ThanhTien = SoLuong * DonGia;
            TongTien = this.TongTien;
            TongSoLuong = this.TongSoLuong;
        }

    }
}