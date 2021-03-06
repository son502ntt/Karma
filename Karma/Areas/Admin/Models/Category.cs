using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Models
{
    public class Category
    {
        [Display(Name = "Mã loại")]
        public string MaLoai { get; set; }
        [Display(Name = "Tên loại")]
        public string TenLoai { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
}