using WebSitem.Models;
using WebSitem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebSitem.Controllers
{

    public class DefaultLoginController : BaseController
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHelper.GetSession() == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Home/Index");
            }
            else
                base.OnActionExecuting(filterContext);
        }
    }
}