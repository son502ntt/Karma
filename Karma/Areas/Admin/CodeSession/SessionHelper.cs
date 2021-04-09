using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karma.Areas.Admin.CodeSession
{
    public class SessionHelper
    {
        public static void SetSession(UserSession userSession)
        {
            HttpContext.Current.Session["User"] = userSession;
        }
        public static UserSession GetSession()
        {
            var userSession = HttpContext.Current.Session["User"];
            if (userSession == null)
                return null;
            else
                return userSession as UserSession;
        }
    }
}