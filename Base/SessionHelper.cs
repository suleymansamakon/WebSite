using WebSitem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSitem
{
    public class SessionHelper
    {

        public static void SetSession(USERS userAccount)
        {
            HttpContext.Current.Session["USERS"] = userAccount;
        }

        public static void KillSession()
        {
            HttpContext.Current.Session["USERS"] = null;
        }

        public static USERS GetSession()
        {
            var sessionObject = HttpContext.Current.Session["USERS"];
            if (sessionObject != null)
            {
                return (USERS)sessionObject;
            }
            else
                return null;
        }

        public static bool IsAdmin()
        {
            var user = SessionHelper.GetSession();
            if (user != null)
                if (user.USER_ROLE_ID == 1)
                    return true;


            return false;
        }

        public static bool IsCustomer()
        {
            var user = SessionHelper.GetSession();
            if (user != null)
                if (user.USER_ROLE_ID == 2)
                    return true;


            return false;
        }
        public static bool IsProjectManager()
        {
            var user = SessionHelper.GetSession();
            if (user != null)
                if (user.USER_ROLE_ID == 3)
                    return true;


            return false;
        }

    }
}