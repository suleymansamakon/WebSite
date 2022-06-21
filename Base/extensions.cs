using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSitem.Base
{
    public static class extentions
    {
        public static MvcHtmlString ActionLinkCustom(this HtmlHelper htmlHelper,
            string linktext,
            string url,
            string class_name = null,
            string font_awesome = null // user -> fa fa-user
            )
        {
            string linkHtml = "";
            string fa_Html = "";
            if (font_awesome != null)
            {
                fa_Html = "<i class='fa fa-" + font_awesome + "'></i> ";
            }

            linkHtml = "<a class='" + class_name + "' href='" + url + "'>" + fa_Html + linktext + " </a>";

            return new MvcHtmlString(linkHtml);
        }

        public static SelectList Nullable(this SelectList slist, string optionalName = "---",
            string optionVal = "0")
        {
            var items = slist.ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = optionalName,
                Value = optionVal
            });

            var sNew = new SelectList(items, slist.DataValueField, slist.DataTextField, slist.SelectedValue);

            return sNew;
        }

    }
}