using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSitem.Models.Entity;

namespace WebSitem.Controllers
{
    public class AUTHORIZATIONsController : ProjectManagerController
    {
        private BLOGDBEntities db = new BLOGDBEntities();

        // GET: AUTHORIZATIONs
        public ActionResult Index()
        {
            return View(db.AUTHORIZATION.ToList());
        }

        // GET: AUTHORIZATIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTHORIZATION aUTHORIZATION = db.AUTHORIZATION.Find(id);
            if (aUTHORIZATION == null)
            {
                return HttpNotFound();
            }
            return View(aUTHORIZATION);
        }

        // GET: AUTHORIZATIONs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AUTHORIZATIONs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AUTHORIZATION_ID,AUTHORIZATION_NAME")] AUTHORIZATION aUTHORIZATION)
        {
            if (ModelState.IsValid)
            {
                db.AUTHORIZATION.Add(aUTHORIZATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aUTHORIZATION);
        }

        // GET: AUTHORIZATIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTHORIZATION aUTHORIZATION = db.AUTHORIZATION.Find(id);
            if (aUTHORIZATION == null)
            {
                return HttpNotFound();
            }
            return View(aUTHORIZATION);
        }

        // POST: AUTHORIZATIONs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AUTHORIZATION_ID,AUTHORIZATION_NAME")] AUTHORIZATION aUTHORIZATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aUTHORIZATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aUTHORIZATION);
        }

        // GET: AUTHORIZATIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTHORIZATION aUTHORIZATION = db.AUTHORIZATION.Find(id);
            if (aUTHORIZATION == null)
            {
                return HttpNotFound();
            }
            return View(aUTHORIZATION);
        }

        // POST: AUTHORIZATIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AUTHORIZATION aUTHORIZATION = db.AUTHORIZATION.Find(id);
            db.AUTHORIZATION.Remove(aUTHORIZATION);
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
    }
}
