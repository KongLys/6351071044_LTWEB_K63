using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities2 db = new QLBANSACHEntities2();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới (ad)
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            // Đưa dữ liệu vào dropdownlist
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            // Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Contain/img/product"), fileName);

                    // Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }

                    sach.Anhbia = fileName;
                    sach.Mota = HttpUtility.HtmlAttributeEncode(sach.Mota);
                    // Lưu vào CSDL
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult Chitietsach(int id)
        {
            // Lấy ra đối tượng sách theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);

            ViewBag.Masach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            // Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }
        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            // Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.SACHes.Remove(sach);
            db.SaveChanges();

            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            // Lấy ra đối tượng sách theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            // Đưa dữ liệu vào dropdownlist
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            // Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Contain/img/product"), fileName);

                    // Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }

                    // Cập nhật lại đường dẫn ảnh mới vào đối tượng sách
                    sach.Anhbia = fileName;

                    // Lưu vào CSDL
                    UpdateModel(sach);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult nhaxuatban()
        {
            return View();
        }
        public ActionResult Sanpham()
        {
            return View();
        }
    }
}