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

    public class BaseController : Controller
    {

        public BLOGDBEntities db = new WebSitem.Models.Entity.BLOGDBEntities();

        public SelectList NullableSelectList(SelectList slist,
            string optionalName = "Seçiniz",
            string optionVal = null)
        {

            var items = slist.ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = optionalName,
                Value = optionVal
            });
            var sNew = new SelectList(items);

            return sNew;
        }
    }
}