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
    public class ROLEsController : ProjectManagerController
    {
        private BLOGDBEntities db = new BLOGDBEntities();

        // GET: ROLEs
        public ActionResult Index()
        {
            return View(db.ROLE.ToList());
        }

        // GET: ROLEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE rOLE = db.ROLE.Find(id);
            if (rOLE == null)
            {
                return HttpNotFound();
            }
            return View(rOLE);
        }

        // GET: ROLEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ROLEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ROLE_ID,ROLE_NAME,ROLE_DELETED")] ROLE rOLE)
        {
            if (ModelState.IsValid)
            {
                db.ROLE.Add(rOLE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rOLE);
        }

        // GET: ROLEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE rOLE = db.ROLE.Find(id);
            if (rOLE == null)
            {
                return HttpNotFound();
            }
            return View(rOLE);
        }

        // POST: ROLEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ROLE_ID,ROLE_NAME,ROLE_DELETED")] ROLE rOLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rOLE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rOLE);
        }

        // GET: ROLEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE rOLE = db.ROLE.Find(id);
            if (rOLE == null)
            {
                return HttpNotFound();
            }
            return View(rOLE);
        }

        // POST: ROLEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ROLE rOLE = db.ROLE.Find(id);
            db.ROLE.Remove(rOLE);
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
