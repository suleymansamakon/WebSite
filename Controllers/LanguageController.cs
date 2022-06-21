using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebSitem.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index(string LanguageAbbrevation)
        {
            if (!String.IsNullOrEmpty(LanguageAbbrevation))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = LanguageAbbrevation;
                cookie.Expires = DateTime.MaxValue;
                Response.Cookies.Add(cookie);
            }
            string url = this.Request.UrlReferrer.AbsolutePath;

            return Redirect(url);
            //return RedirectToAction("Index","Home");
        }
    }
}