using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class SignUp
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public string NgaySinh { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public int SDT { get; set; }
        public string Avatar { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Xác nhận mật khẩu")]
        //[Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp, vui lòng thử lại!")]
        public string ConfirmPassword { get; set; }
        public string CodeActive { get; set; }
    }
}