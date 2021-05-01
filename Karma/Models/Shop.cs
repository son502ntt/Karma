using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Models
{
    public class Shop
    {
        public List<Categories> Categories = new List<Categories>();
        public List<Products> Products = new List<Products>();
    }
}