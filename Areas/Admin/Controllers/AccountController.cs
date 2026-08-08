using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        // GET: Admin/Account
        public ActionResult Settings()
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            int matk = (int)session.UserID;
            var user = (from tk in db.TAIKHOANs
                        join kh in db.KHACHHANGs on tk.MATK equals kh.MATK into khJoin
                        from kh in khJoin.DefaultIfEmpty()
                        join ltk in db.LOAITKs on tk.MALOAITK equals ltk.MALOAITK
                        where tk.MATK == matk
                        select new AccountSettingsViewModel
                        {
                            MATK = tk.MATK,
                            TENDN = tk.TENDN,
                            TENKH = kh != null ? kh.TENKH : "",
                            EMAIL = kh != null ? kh.EMAIL : "",
                            SDT = kh != null ? kh.SDT : "",
                            GIOITINH = kh != null ? kh.GIOITINH : "",
                            DIACHI = kh != null ? kh.DIACHI : "",
                            NGAYDANGKY = tk.NGAYDANGKY,
                            TRANGTHAI = tk.TRANGTHAI,
                            LOAITK = ltk.TENLOAITK
                        }).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(AccountSettingsViewModel model)
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (ModelState.IsValid)
            {
                var tk = db.TAIKHOANs.Find(model.MATK);
                if (tk != null)
                {
                    tk.TRANGTHAI = model.TRANGTHAI;
                    var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == tk.MATK);
                    if (kh != null)
                    {
                        kh.TENKH = model.TENKH;
                        kh.EMAIL = model.EMAIL;
                        kh.SDT = model.SDT;
                        kh.GIOITINH = model.GIOITINH;
                        kh.DIACHI = model.DIACHI;
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Cập nhật thành công!";
                }
                else
                {
                    ViewBag.Message = "Tài khoản không tồn tại!";
                }
            }

            // Load lại dữ liệu mới
            var updatedUser = (from t in db.TAIKHOANs
                               join k in db.KHACHHANGs on t.MATK equals k.MATK into khJoin
                               from k in khJoin.DefaultIfEmpty()
                               join l in db.LOAITKs on t.MALOAITK equals l.MALOAITK
                               where t.MATK == model.MATK
                               select new AccountSettingsViewModel
                               {
                                   MATK = t.MATK,
                                   TENDN = t.TENDN,
                                   TENKH = k != null ? k.TENKH : "",
                                   EMAIL = k != null ? k.EMAIL : "",
                                   SDT = k != null ? k.SDT : "",
                                   GIOITINH = k != null ? k.GIOITINH : "",
                                   DIACHI = k != null ? k.DIACHI : "",
                                   NGAYDANGKY = t.NGAYDANGKY,
                                   TRANGTHAI = t.TRANGTHAI,
                                   LOAITK = l.TENLOAITK
                               }).FirstOrDefault();
            return View(updatedUser);
        }

        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            int matk = (int)session.UserID;
            var user = (from tk in db.TAIKHOANs
                        join kh in db.KHACHHANGs on tk.MATK equals kh.MATK into khJoin
                        from kh in khJoin.DefaultIfEmpty()
                        join ltk in db.LOAITKs on tk.MALOAITK equals ltk.MALOAITK
                        where tk.MATK == matk
                        select new AccountSettingsViewModel
                        {
                            MATK = tk.MATK,
                            TENDN = tk.TENDN,
                            TENKH = kh != null ? kh.TENKH : "",
                            EMAIL = kh != null ? kh.EMAIL : "",
                            SDT = kh != null ? kh.SDT : "",
                            GIOITINH = kh != null ? kh.GIOITINH : "",
                            DIACHI = kh != null ? kh.DIACHI : "",
                            NGAYDANGKY = tk.NGAYDANGKY,
                            TRANGTHAI = tk.TRANGTHAI,
                            LOAITK = ltk.TENLOAITK
                        }).FirstOrDefault();
            return View(user);
        }
    }
}