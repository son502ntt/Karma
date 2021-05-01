using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.Models
{
    public class Customer
    {
        [Display(Name = "ID")]
        public string ID { get; set; }
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }
        [Display(Name = "Ngày sinh")]
        public string NgaySinh { get; set; }
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Số ddienj thoại")]
        public string SDT { get; set; }
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
    }
}