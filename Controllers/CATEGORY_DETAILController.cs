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
    public class CATEGORY_DETAILController : ProjectManagerController
    {
        private BLOGDBEntities db = new BLOGDBEntities();

        // GET: CATEGORY_DETAIL
        public ActionResult Index()
        {
            var cATEGORY_DETAIL = db.CATEGORY_DETAIL.Include(c => c.BLOG).Include(c => c.CATEGORY);
            return View(cATEGORY_DETAIL.ToList());
        }

        // GET: CATEGORY_DETAIL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORY_DETAIL cATEGORY_DETAIL = db.CATEGORY_DETAIL.Find(id);
            if (cATEGORY_DETAIL == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORY_DETAIL);
        }

        // GET: CATEGORY_DETAIL/Create
        public ActionResult Create()
        {
            ViewBag.BLOG_ID = new SelectList(db.BLOG, "BLOG_ID", "BLOG_NAME");
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORY, "CATEGORY_ID", "CATEGORY_NAME");
            return View();
        }

        // POST: CATEGORY_DETAIL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CATEGORY_DETAIL_ID,CATEGORY_ID,BLOG_ID")] CATEGORY_DETAIL cATEGORY_DETAIL)
        {
            if (ModelState.IsValid)
            {
                db.CATEGORY_DETAIL.Add(cATEGORY_DETAIL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BLOG_ID = new SelectList(db.BLOG, "BLOG_ID", "BLOG_NAME", cATEGORY_DETAIL.BLOG_ID);
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORY, "CATEGORY_ID", "CATEGORY_NAME", cATEGORY_DETAIL.CATEGORY_ID);
            return View(cATEGORY_DETAIL);
        }

        // GET: CATEGORY_DETAIL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORY_DETAIL cATEGORY_DETAIL = db.CATEGORY_DETAIL.Find(id);
            if (cATEGORY_DETAIL == null)
            {
                return HttpNotFound();
            }
            ViewBag.BLOG_ID = new SelectList(db.BLOG, "BLOG_ID", "BLOG_NAME", cATEGORY_DETAIL.BLOG_ID);
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORY, "CATEGORY_ID", "CATEGORY_NAME", cATEGORY_DETAIL.CATEGORY_ID);
            return View(cATEGORY_DETAIL);
        }

        // POST: CATEGORY_DETAIL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CATEGORY_DETAIL_ID,CATEGORY_ID,BLOG_ID")] CATEGORY_DETAIL cATEGORY_DETAIL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cATEGORY_DETAIL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BLOG_ID = new SelectList(db.BLOG, "BLOG_ID", "BLOG_NAME", cATEGORY_DETAIL.BLOG_ID);
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORY, "CATEGORY_ID", "CATEGORY_NAME", cATEGORY_DETAIL.CATEGORY_ID);
            return View(cATEGORY_DETAIL);
        }

        // GET: CATEGORY_DETAIL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORY_DETAIL cATEGORY_DETAIL = db.CATEGORY_DETAIL.Find(id);
            if (cATEGORY_DETAIL == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORY_DETAIL);
        }

        // POST: CATEGORY_DETAIL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CATEGORY_DETAIL cATEGORY_DETAIL = db.CATEGORY_DETAIL.Find(id);
            db.CATEGORY_DETAIL.Remove(cATEGORY_DETAIL);
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
