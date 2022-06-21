using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebSitem.Models.Entity;

namespace WebSitem.Controllers
{
    public class BLOGsController : ProjectManagerController
    {
        private BLOGDBEntities db = new BLOGDBEntities();

        // GET: BLOGs
        public ActionResult Index()
        {
            var bLOG = db.BLOG.Include(b => b.LANGUAGE).Include(b => b.USERS);
            return View(bLOG.Where(x => x.BLOG_DELETED != true).ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BLOG bLOG = db.BLOG.Find(id);
            if (bLOG == null)
            {
                return HttpNotFound();
            }
            return View(bLOG);
        }

        // GET: BLOGs/Create
        public ActionResult Create()
        {
            ViewBag.BLOG_LANGUAGE_CODE = new SelectList(db.LANGUAGE, "LANGUAGE_CODE", "NAME");
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME");
            return View();
        }

        // POST: BLOGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BLOG_ID,BLOG_NAME,SEO_URL,BLOG_CONTENT,USER_ID,DATE_CREATED,BLOG_LIKE_COUNT,BLOG_COMMENT_COUNT,BLOG_READ_NUMBER,BLOG_DELETED,BLOG_LANGUAGE_CODE")] BLOG bLOG)
        {
            if (ModelState.IsValid)
            {
                new DataColumn("DATE_CREATED", typeof(DateTime));
                bLOG.DATE_CREATED = System.DateTime.Now;
                if (bLOG.USER_ID == 0)
                {
                    bLOG.USER_ID = SessionHelper.GetSession().USER_ID;
                }

                db.BLOG.Add(bLOG);
                db.SaveChanges();
                var id = bLOG.BLOG_ID;
                var methodName = MethodBase.GetCurrentMethod().Name;
                //var olan metodun ismi değişkene atandı
                sendMail(id, methodName);
                //sendMail metoduna blogID ve metod ismi yollandı.
                return RedirectToAction("Index");
            }

            ViewBag.BLOG_LANGUAGE_CODE = new SelectList(db.LANGUAGE, "LANGUAGE_CODE", "NAME", bLOG.BLOG_LANGUAGE_CODE);
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", bLOG.USER_ID);
            return View(bLOG);
        }

        // GET: BLOGs/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BLOG bLOG = db.BLOG.Find(id);
            if (bLOG == null)
            {
                return HttpNotFound();
            }
            ViewBag.BLOG_LANGUAGE_CODE = new SelectList(db.LANGUAGE, "LANGUAGE_CODE", "NAME", bLOG.BLOG_LANGUAGE_CODE);
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", bLOG.USER_ID);
            return View(bLOG);
        }

        // POST: BLOGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BLOG_ID,BLOG_NAME,SEO_URL,BLOG_CONTENT,USER_ID,DATE_CREATED,BLOG_LIKE_COUNT,BLOG_COMMENT_COUNT,BLOG_READ_NUMBER,BLOG_DELETED,BLOG_LANGUAGE_CODE")] BLOG bLOG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bLOG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BLOG_LANGUAGE_CODE = new SelectList(db.LANGUAGE, "LANGUAGE_CODE", "NAME", bLOG.BLOG_LANGUAGE_CODE);
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USERNAME", bLOG.USER_ID);
            return View(bLOG);
        }

        // GET: BLOGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BLOG bLOG = db.BLOG.Find(id);
            if (bLOG == null)
            {
                return HttpNotFound();
            }
            return View(bLOG);
        }

        // POST: BLOGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            BLOG bLOG = db.BLOG.Find(id);
            //db.BLOG.Remove(bLOG);
            bLOG.BLOG_DELETED = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public void sendMail(int? id, string methodName)
        {
            var description = "s";
            var blogID = db.BLOG.FirstOrDefault(x => x.BLOG_ID == id); //blog id değeri bulundu
            var blogName = blogID.BLOG_NAME; //blog ismi çekildi.
            var blogUser = db.USERS.FirstOrDefault(x => x.USER_ID == blogID.USER_ID); //blog yazısını hangi kullanıcının yazdığı tespit edildi.
            var email = blogUser.E_MAİL; //kullanıcı email bilgisi çekildi.
            var name = blogUser.FIRST_NAME + " " + blogUser.LAST_NAME;
            if (methodName == "Create")
            {
                description = "Sayın " + name + " " +
                    "kodumunblogu.com sitesinde sizin adınıza " + blogName + " adlı blog yazısı paylaşılmıştır.";
            }
            using (var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("suleymansamakon@gmail.com", "sifre"),
                EnableSsl = true
            })
            {
                client.Send("suleymansamakon@gmail.com", email, name, description);
            }


        }
    }
}
