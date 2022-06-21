using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebSitem.Models.Entity;

namespace WebSitem.Controllers
{
    
    public class HomeController : Controller
    {
        BLOGDBEntities db = new BLOGDBEntities();
        public ActionResult Index()
        {
           
            if (SessionHelper.IsProjectManager()  || SessionHelper.IsAdmin())
            {
                return View("IndexProjectManager");
            }
           
            List<BLOG> makaleler = db.BLOG.OrderByDescending(s => s.BLOG_ID).Where(s => s.BLOG_DELETED == false).Take(5).ToList();

            return View(makaleler);
        }
        public ActionResult IndexProjectManager()
        {
            return View();
        }
        public PartialViewResult DurumPartial()
        {
            return PartialView(new Durum().durumModeli());
        }
        public PartialViewResult userCount()
        {
            int number = db.USERS.Count();
            ViewData["number"] = number;
            return PartialView();
        }

    
        public ActionResult MakaleDetay(int? id)
        {//bu action devamını oku butonu için açılmıştır ancak substring hatası ile şimdililk kullanım dışıdır.
            BLOG bLOG = db.BLOG.Where(s => s.BLOG_ID == id).SingleOrDefault();
            if (bLOG == null)
            {
                return RedirectToAction("Index");

            }
            return View(bLOG);
        }
        public ActionResult Kategoriler()
        {
            List<CATEGORY> kategoriler = db.CATEGORY.ToList();
          //  var deneme = db.;
           // ViewBag.Sayi = deneme;
            return PartialView(kategoriler);
        }
        public ActionResult KategoriBlog(int? id)
        {
            var deneme = db.Database.SqlQuery<BLOG>("SELECT B.* FROM BLOG B JOIN CATEGORY_DETAIL CD ON B.BLOG_ID=CD.BLOG_ID JOIN USERS U ON B.USER_ID=U.USER_ID WHERE CD.CATEGORY_ID=@id", new SqlParameter("@id", id)).OrderByDescending(s => s.BLOG_ID).Where(s => s.BLOG_DELETED == false);

            //    List<BLOG> makale = db.CATEGORY_DETAIL.Where(s => s.CATEGORY_ID == id).ToList();
            //    db.CATEGORY_DETAIL.Where(a => a.CATEGORY_ID == id)

            return View(deneme);
        }
        public ActionResult SonEklenenler()
        {
            List<BLOG> blogisim = db.BLOG.OrderByDescending(s => s.BLOG_ID).Where(s => s.BLOG_DELETED == false).Take(5).ToList();

            return PartialView(blogisim);
        }
        public ActionResult SonEklenenGoster(int? id)
        {
            List<BLOG> sonEklenen = db.BLOG.Where(s => s.BLOG_ID == id).Where(s => s.BLOG_DELETED == false).ToList();
            return View(sonEklenen);
        }
        public ActionResult SearchBlog(string searching)
        {
            return View(db.BLOG.Where(x => x.BLOG_NAME.Contains(searching) || searching == null).ToList());
        }
        public ActionResult SocialMedia()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            
            return View();

        }
        [HttpPost]
        public ActionResult Contact(FormCollection formCollection)
        {
            var name = formCollection["name"];
            var email = formCollection["email"];
            var text = formCollection["text"];
            if (name != "" || email != "" || text != "")
            {
                text = "Gönderici ismi : " + name + " Gönderici E mail : " + email + "Gönderici Mesajı :" + text;
                using (var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("suleymansamakon@gmail.com", "sifre"),
                    EnableSsl = true
                })
                {
                    client.Send(email, "suleymansamakon@gmail.com", name, text);
                }
                ViewBag.Message = "Mail başarılı bir şekilde gönderilmiştir.";
                return RedirectToAction("Contact");
            }
            else
            {
                ViewBag.Error = "Bir hata oluştu.";
            }
            return View();

        }
        

    }
}