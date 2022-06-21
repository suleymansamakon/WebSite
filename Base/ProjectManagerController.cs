﻿using WebSitem.Models;
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

    public class ProjectManagerController : DefaultLoginController
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(SessionHelper.IsProjectManager() || SessionHelper.IsAdmin()))
            {
                filterContext.HttpContext.Response.Redirect("~/Home");
            }
            else
                base.OnActionExecuting(filterContext);
        }
    }
}