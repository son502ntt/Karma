using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Cart
    {
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public string TenSanPham { get; set; }
        public string GhiChu { get; set; }
        public double? DonGia { get; set; }
        public double? ThanhTien { get; set; }
        public double? TongTien { get; set; }
        public int TongSoLuong { get; set; }
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