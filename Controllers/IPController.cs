using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSitem.Models;

namespace WebSitem.Controllers
{
    public class IPController : Controller
    {
       
        public ActionResult Index()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            LocationModel location = new LocationModel();
            string url = string.Format("http://freegeoip.net/json/{0}", ipAddress);
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                location = new JavaScriptSerializer().Deserialize<LocationModel>(json);
            }

            return View(location);
        }
    }
}