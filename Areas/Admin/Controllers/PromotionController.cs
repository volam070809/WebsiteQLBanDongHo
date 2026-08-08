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
    public class PromotionController : Controller
    {
        PromotionService promotionService = new PromotionService();
        // GET: Admin/Promotion
        public ActionResult Index()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (TempData["StatusMessage"] != null)
            {
                ViewBag.message = TempData["StatusMessage"];
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.error = TempData["ErrorMessage"];
            }
            return View(promotionService.getAllPromotion());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            PromotionViewModel promotionViewModel = new PromotionViewModel();
            promotionViewModel.MAKM = newMAKM(promotionService.getLastRecord());
            return View(promotionViewModel);
        }

        [HttpPost]
        public ActionResult Create(PromotionViewModel km)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (ModelState.IsValid)
            {
                KHUYENMAI khuyenmai = new KHUYENMAI();
                khuyenmai.MAKM = km.MAKM;
                khuyenmai.TENKM = km.TENKM;
                khuyenmai.NGAYBD = km.NGAYBD;
                khuyenmai.NGAYKT = km.NGAYKT;
                if (promotionService.addPromotion(khuyenmai))
                {
                    TempData["StatusMessage"] = $"Thêm mới khuyến mãi {km.TENKM} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Thêm mới khuyến mãi thất bại. Có thể mã hoặc tên khuyến mãi đã tồn tại.";
                }
            }
            ViewBag.error = ViewBag.error ?? "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại.";
            return View(km);
        }

        [HttpGet]
        public ActionResult Update(string makm)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            var km = promotionService.getPromotionById(makm);
            if (km == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khuyến mãi này.";
                return RedirectToAction("Index");
            }

            PromotionViewModel promotionViewModel = new PromotionViewModel();
            promotionViewModel.MAKM = km.MAKM;
            promotionViewModel.TENKM = km.TENKM;
            promotionViewModel.NGAYBD = (DateTime)km.NGAYBD;
            promotionViewModel.NGAYKT = (DateTime)km.NGAYKT;
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.error = TempData["ErrorMessage"];
            }
            return View(promotionViewModel);
        }

        [HttpPost]
        public ActionResult Update(PromotionViewModel km)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (ModelState.IsValid)
            {
                KHUYENMAI khuyenmai = new KHUYENMAI();
                khuyenmai.MAKM = km.MAKM;
                khuyenmai.TENKM = km.TENKM;
                khuyenmai.NGAYBD = km.NGAYBD;
                khuyenmai.NGAYKT = km.NGAYKT;
                if (promotionService.updatePromotion(khuyenmai))
                {
                    TempData["StatusMessage"] = $"Cập nhật khuyến mãi {km.TENKM} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật khuyến mãi thất bại. Vui lòng thử lại.";
                    return RedirectToAction("Update", new { makm = km.MAKM });
                }
            }
            ViewBag.error = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại.";
            return View(km);
        }

        public ActionResult Delete(string makm)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            bool result = promotionService.deletePromotion(makm);
            if (result)
            {
                TempData["StatusMessage"] = $"Xóa khuyến mãi có mã {makm} thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Xóa khuyến mãi có mã {makm} thất bại. Có thể khuyến mãi đang được sử dụng.";
            }
            return RedirectToAction("Index");
        }

        public string newMAKM(string lastMAKM)
        {
            string res = "KM00001";
            if (String.Compare(lastMAKM, "", false) != 0)
            {
                int tam = Int32.Parse(lastMAKM.Substring(2)) + 1;
                string rs = tam.ToString();
                while (rs.Length < 5)
                {
                    rs = "0" + rs;
                }
                res = "KM" + rs;
            }
            return res;
        }
    }
}