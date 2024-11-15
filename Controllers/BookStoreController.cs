using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;
using PagedList;
using PagedList.Mvc;
namespace WebBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        private QLBANSACHEntities2 db = new QLBANSACHEntities2();

        private List<SACH> LaySachMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sachMoi = LaySachMoi(15);
            return View(sachMoi.ToPagedList(pageNum, pageSize));
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
        public ActionResult SachByTopicDetails(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sach = db.SACHes
            .OrderByDescending(a => a.Ngaycapnhat)
            .Where(a => a.MaCD == id)
            .Take(pageSize)
            .ToList();
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewData["Id"] = id;
            return View(sach.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SachByPublisherDetails(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sach = db.SACHes
            .OrderByDescending(a => a.Ngaycapnhat)
            .Where(a => a.MaNXB == id)
            .Take(pageSize)
            .ToList();
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewData["Id"] = id;
            return View(sach.ToPagedList(pageNum, pageSize));
        }
    }
}
