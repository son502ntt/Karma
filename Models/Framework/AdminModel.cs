using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.Framework
{
    public class AdminModel
    {
        private DbConextShop context = null;
        public AdminModel()
        {
            context = new DbConextShop();
        }
        public bool? Login (string userName, string passWord)
        {
            object[] sqlParams = 
            {
                new SqlParameter("@UserName", userName),
                new SqlParameter("@PassWord", passWord),
            };
            var res = 
                context.Database.SqlQuery<bool>("Sp_Admin_Login @UserName, @PassWord", sqlParams).SingleOrDefault();
            return res;
        }
    }
}
