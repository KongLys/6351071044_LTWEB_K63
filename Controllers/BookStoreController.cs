using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        private QLBANSACHEntities1 db = new QLBANSACHEntities1();

        // GET: BookStore
        public ActionResult Index()
        {
            var sACHes = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);
            return View(sACHes.ToList());
        }

        // GET: BookStore/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // GET: BookStore/Create
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB");
            return View();
        }

        // POST: BookStore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Masach,Tensach,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaCD,MaNXB")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.SACHes.Add(sACH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: BookStore/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // POST: BookStore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Masach,Tensach,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaCD,MaNXB")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sACH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: BookStore/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // POST: BookStore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            db.SACHes.Remove(sACH);
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

        public ActionResult CategoryList()
        {
            var data = db.CHUDEs.ToList();
            return PartialView(data);
        }
        public ActionResult NhaXuatBanList()
        {
            var data = db.NHAXUATBANs.ToList();
            return PartialView(data);
        }
        public ActionResult SachByTopicDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sach = from s in db.SACHes where s.MaCD == id select s;
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }
        public ActionResult SachByPublisherDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sach = from s in db.SACHes where s.MaNXB == id select s;
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }
    }
}
