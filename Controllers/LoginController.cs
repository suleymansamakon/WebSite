
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
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            //if (Request.Url.Host.Contains("localhost"))
            //{
            //    var user = db.USERS.FirstOrDefault(x => x.USERNAME == "süleyman" && x.PASSWORD == "ebfebc422");
            //    if (user != null)
            //    {
            //        SessionHelper.SetSession(user);
            //        return RedirectToAction("Index", "Home");
            //    }
            //}

            return View();
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var user = db.USERS.FirstOrDefault(x => x.USERNAME == username && x.PASSWORD == password &&x.IS_EMAIL_VERIFIED!=false);
            if (user != null)
            {
                user.GENDER = GetIp();
                SessionHelper.SetSession(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            }

            return View();
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
            ViewBag.Status = false;
            ViewBag.Message = "";
           
            if (uSER.ConfirmPassword == null)
            {
                uSER.ConfirmPassword = uSER.PASSWORD;
            }
            if (uSER.USER_ROLE_ID == 0)
            {
                //uSER.GENDER = GetIp();
                // bool control= IsIpAdressExist(uSER.GENDER);
                //if (control == true)
                //{
                //    ModelState.AddModelError("IpAdressExist","IP adress already exist.");
                //}
                uSER.USER_ROLE_ID = 2;
                //bu if blogu kayıt olma aşamasında kullanıcıya rolü sorulmadığı için otomatik 0 olarak geliyor bizde kayıt olma sekmesinden
                //kayıt olanları otomatik kullanıcı olarak atıyoruz.
            }
            if (ModelState.IsValid)
            {
                new DataColumn("DATE_CREATED", typeof(DateTime));
                uSER.DATE_CREATED = System.DateTime.Now;
                #region E Mail already exist
                //var isExist = IsMailExist(uSER.E_MAİL);
                //if (isExist)
                //{
                //    ModelState.AddModelError("EmailExist", "E Mail is already exist");
                //    //ilk parametre error adı olarak validation message  olarak USERs/Create içindeki email kısmına yollandı.
                //    return View(uSER);
                //}
                #endregion
                #region Generate Activation Code
                uSER.EMAIL_ACTIVATION_CODE = Guid.NewGuid();
                uSER.IS_EMAIL_VERIFIED = false;
                #endregion

                #region Password Hashing
                //uSER.PASSWORD = Crypto.Hash(uSER.PASSWORD);
                //uSER.ConfirmPassword = Crypto.Hash(uSER.ConfirmPassword);
                #endregion

                db.USERS.Add(uSER);
                db.SaveChanges();
                sendVerificationLinkEmail(uSER.E_MAİL, uSER.EMAIL_ACTIVATION_CODE.ToString());
                ViewBag.Message = "Kayıt Başarılı! Aktivaston kodu email hesabına gönderildi. Email hesabınız : " + uSER.E_MAİL;
                ViewBag.Status = true;
               // return RedirectToAction("Index", "Login");
            }

            ViewBag.Error = "Girilen bilgiler eksik veya hatalı.";
            

            ViewBag.USER_ROLE_ID = new SelectList(db.ROLE, "ROLE_ID", "ROLE_NAME", uSER.USER_ROLE_ID);


            return View(uSER);
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var v = db.USERS.Where(a => a.EMAIL_ACTIVATION_CODE == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IS_EMAIL_VERIFIED = true;
                db.SaveChanges();
                Status = true;
            }
            else
            {
                ViewBag.Message = "invalid request";
            }
            ViewBag.Status = true;
            return View();
        }
        [NonAction]
        public void sendVerificationLinkEmail(string email,string activationCode)
        {
            var verifyUrl = "/Login/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery,verifyUrl);
            var fromEmail = new MailAddress("suleymansamakon@gmail.com",".");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Ebfebc422+12";
            string subject = "Your Account is successfully created!";
            string body = "</br></br>Please click on the below link to verify your account" +
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
                Body = body,
                IsBodyHtml = true
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
        //[NonAction]
        //public bool IsIpAdressExist(string ip)
        //{
        //    var v = db.USERS.Where(a => a.GENDER == ip).FirstOrDefault();
        //    return v != null;
        //}



        public ActionResult LogOut()
        {
            SessionHelper.KillSession();
            return RedirectToAction("Index", "Home");
        }

    }


}
