using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Categories
    {
        [Display(Name = "Mã loại")]
        public string MaLoai { get; set; }
        [Display(Name = "Tên loại")]
        public string TenLoai { get; set; }
    }
}