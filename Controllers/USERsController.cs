using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebSitem.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WebSitem.Controllers
{
    public class USERsController : ProjectManagerController
    {
        private BLOGDBEntities db = new BLOGDBEntities();

        // GET: USERs
        public ActionResult Index()
        {
            var uSER = db.USERS.Include(u => u.ROLE);
            return View(uSER.Where(x=>x.USER_DELETED!=true).ToList());
        }

        // GET: USERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // GET: USERs/Create
        public ActionResult Create()
        {
            ViewBag.USER_ROLE_ID = new SelectList(db.ROLE, "ROLE_ID", "ROLE_NAME");
            return View();
        }

        // POST: USERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,USERNAME,PASSWORD,ConfirmPassword,FIRST_NAME,LAST_NAME,E_MAİL,GENDER,DATE_CREATED,USER_DELETED,USER_ROLE_ID")] USERS uSER)
        {
            string message = "";
            bool Status = false;
         
            if (uSER.ConfirmPassword == null)
            {
                uSER.ConfirmPassword = uSER.PASSWORD;
            }
            if (uSER.USER_ROLE_ID == 0)
            {
                uSER.USER_ROLE_ID = 2;
                //bu if blogu kayıt olma aşamasında kullanıcıya rolü sorulmadığı için otomatik 0 olarak geliyor bizde kayıt olma sekmesinden
                //kayıt olanları otomatik kullanıcı olarak atıyoruz.
            }
            if (ModelState.IsValid)
            {
                new DataColumn("DATE_CREATED", typeof(DateTime));
                uSER.DATE_CREATED = System.DateTime.Now;
                #region E Mail already exist
                var isExist = IsMailExist(uSER.E_MAİL);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "E Mail is already exist");
                    //ilk parametre error adı olarak validation message  olarak USERs/Create içindeki email kısmına yollandı.
                    return View(uSER);
                }
                #endregion

                #region Password Hashing
                //uSER.PASSWORD = Crypto.Hash(uSER.PASSWORD);
                //uSER.ConfirmPassword = Crypto.Hash(uSER.ConfirmPassword);
                #endregion

                db.USERS.Add(uSER);
                db.SaveChanges();
                sendVerificationLinkEmail(uSER.E_MAİL, uSER.EMAIL_ACTIVATION_CODE.ToString());
                message = uSER.E_MAİL + " hesabınıza aktivasyon kodu gönderildi.";
                Status = true;
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Error = "Girilen bilgiler eksik veya hatalı.";
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            ViewBag.USER_ROLE_ID = new SelectList(db.ROLE, "ROLE_ID", "ROLE_NAME", uSER.USER_ROLE_ID);
            return View(uSER);
        }
        [NonAction]
        public void sendVerificationLinkEmail(string eMail, string activationCode)
        {
            var verifyUrl = "/Login/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("suleymansamakon@gmail.com");
            var toEmail = new MailAddress(eMail);
            var fromEmailPassword = "sifre";
            string subject = "Hesabınız başarılı bir şekilde oluşturuldu.";
            string body = "<br><br>Lütfen hesabınızı doğrulamak için linke tıklayınız" +
                "<br><br><a href='" + link + "'>" + link + "</a>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body=body,
                IsBodyHtml=true
            
            }) 
            smtp.Send(message);
        }

        [NonAction]
        public bool IsMailExist(string email)
        {
            using (BLOGDBEntities db = new BLOGDBEntities())
            {
                var v = db.USERS.Where(a => a.E_MAİL == email).FirstOrDefault();
                return v != null;
            }
        }
        

        // GET: USERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_ROLE_ID = new SelectList(db.ROLE, "ROLE_ID", "ROLE_NAME", uSER.USER_ROLE_ID);
            return View(uSER);
        }

        // POST: USERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,USERNAME,ConfirmPassword,PASSWORD,FIRST_NAME,LAST_NAME,E_MAİL,GENDER,DATE_CREATED,USER_DELETED,USER_ROLE_ID")] USERS uSER)
            {
            if (uSER.ConfirmPassword == null)
            {
                uSER.ConfirmPassword = uSER.PASSWORD;
            }
            if (ModelState.IsValid)
            {
                
                db.Entry(uSER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.USER_ROLE_ID = new SelectList(db.ROLE, "ROLE_ID", "ROLE_NAME", uSER.USER_ROLE_ID);

            return View(uSER);
        }

        // GET: USERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // POST: USERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            USERS uSER = db.USERS.Find(id);
            //db.USERS.Remove(uSER);
            //confirm password null döndüğü için password değeri atandı yoksa validation hatası veriyordu.
            uSER.ConfirmPassword = uSER.PASSWORD;
            uSER.USER_DELETED = true;
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                    Response.End();
                }
            }
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
    }
}
