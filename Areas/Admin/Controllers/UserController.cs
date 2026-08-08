using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        private UserLogin RequireAdminSession()
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                Response.Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl), endResponse: true);
            }
            return session;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            RequireAdminSession();

            var users = db.TAIKHOANs
                .Include(x => x.LOAITK)
                .OrderByDescending(x => x.NGAYDANGKY)
                .ToList();

            return View(users);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            RequireAdminSession();

            var u = db.TAIKHOANs.SingleOrDefault(x => x.MATK == id);
            if (u == null) return HttpNotFound();

            ViewBag.Roles = new SelectList(db.LOAITKs.OrderBy(r => r.TENLOAITK).ToList(), "MALOAITK", "TENLOAITK", u.MALOAITK);

            var vm = new UserEditViewModel
            {
                MATK = u.MATK,
                TENDN = u.TENDN,
                MALOAITK = u.MALOAITK,
                TRANGTHAI = u.TRANGTHAI ?? true
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel model)
        {
            RequireAdminSession();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(db.LOAITKs.OrderBy(r => r.TENLOAITK).ToList(), "MALOAITK", "TENLOAITK", model.MALOAITK);
                return View(model);
            }

            var u = db.TAIKHOANs.SingleOrDefault(x => x.MATK == model.MATK);
            if (u == null) return HttpNotFound();

            // Tránh trùng username
            var exists = db.TAIKHOANs.Any(x => x.MATK != model.MATK && x.TENDN == model.TENDN);
            if (exists)
            {
                ModelState.AddModelError("TENDN", "Tên đăng nhập đã tồn tại");
                ViewBag.Roles = new SelectList(db.LOAITKs.OrderBy(r => r.TENLOAITK).ToList(), "MALOAITK", "TENLOAITK", model.MALOAITK);
                return View(model);
            }

            u.TENDN = (model.TENDN ?? "").Trim();
            u.MALOAITK = model.MALOAITK;
            u.TRANGTHAI = model.TRANGTHAI;

            db.SaveChanges();
            TempData["Success"] = "Cập nhật tài khoản thành công.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            RequireAdminSession();
            var u = db.TAIKHOANs.SingleOrDefault(x => x.MATK == id);
            if (u == null) return HttpNotFound();

            var vm = new ResetPasswordViewModel
            {
                MATK = u.MATK,
                UserName = u.TENDN
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            RequireAdminSession();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var u = db.TAIKHOANs.SingleOrDefault(x => x.MATK == model.MATK);
            if (u == null) return HttpNotFound();

            // Lưu theo MD5 để tương thích login hiện tại
            u.MATKHAU = Encryptor.MD5Hash(model.NewPassword ?? "");
            db.SaveChanges();

            TempData["Success"] = "Reset mật khẩu thành công.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var session = RequireAdminSession();
            if (session != null && session.UserID == id)
            {
                TempData["Error"] = "Không thể xoá tài khoản đang đăng nhập.";
                return RedirectToAction("Index");
            }

            var u = db.TAIKHOANs.SingleOrDefault(x => x.MATK == id);
            if (u == null)
            {
                TempData["Error"] = "Tài khoản không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                db.TAIKHOANs.Remove(u);
                db.SaveChanges();
                TempData["Success"] = "Đã xoá tài khoản.";
            }
            catch (Exception)
            {
                // Nếu vướng ràng buộc dữ liệu: khoá tài khoản thay vì crash
                u.TRANGTHAI = false;
                db.SaveChanges();
                TempData["Error"] = "Không thể xoá do tài khoản đã phát sinh dữ liệu. Hệ thống đã khoá tài khoản.";
            }

            return RedirectToAction("Index");
        }
    }
}
