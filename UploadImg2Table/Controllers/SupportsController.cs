using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using UploadImg2Table.Models;

namespace UploadImg2Table.Controllers
{
    public class SupportsController : Controller
    {
        private SupportContext db = new SupportContext();

        // GET: Supports
        public ActionResult Index()
        {
            return View(db.Supports.ToList());
        }

        // GET: Supports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            var images = new List<FileDetail>();
                        images = db.FileDetails
                  .Where(x => x.SupportId == id)
                  .ToList();
            ViewBag.file = images;
            return View(support);
        }

        // GET: Supports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupportId,Name,Summary")] Support support)
        {   
            if (ModelState.IsValid)
            {
                List<FileDetail> fileDetails = new List<FileDetail>();
                for(int i = 0; i <Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if(file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        FileDetail fileDetail = new FileDetail() {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName)                           
                        };
                        fileDetails.Add(fileDetail);

                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(path);

                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 200)
                            img.Resize(200, 200);
                        img.Save(path);
                    }
                }
                support.FileDetail = fileDetails;
                db.Supports.Add(support);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(support);
        }

        // GET: Supports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            return View(support);
        }

        // POST: Supports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupportId,Name,Summary")] Support support)
        {
            if (ModelState.IsValid)
            {
                db.Entry(support).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(support);
        }

        // GET: Supports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            return View(support);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Support support = db.Supports.Find(id);
            db.Supports.Remove(support);
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
