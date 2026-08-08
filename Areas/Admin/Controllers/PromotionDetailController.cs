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
    public class PromotionDetailController : Controller
    {
        PromotionDetailService promotionDetailService = new PromotionDetailService();
        // GET: Admin/PromotionDetail
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
            return View(promotionDetailService.getAllPromotionDetail());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            PromotionDetailViewModel promotionDetailViewModel = new PromotionDetailViewModel();
            ViewBag.KHUYENMAIx = promotionDetailService.getALLPromotion();
            ViewBag.SANPHAMx = promotionDetailService.getAllProduct();
            return View(promotionDetailViewModel);
        }

        [HttpPost]
        public ActionResult Create(PromotionDetailViewModel ctkm)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (ModelState.IsValid)
            {
                CHITIETKM chitietkm = new CHITIETKM();
                chitietkm.MAKM = ctkm.MAKM;
                chitietkm.MASP = ctkm.MASP;
                chitietkm.PHANTRAMKM = ctkm.PHANTRAMKM;
                if (promotionDetailService.addPromotionDetail(chitietkm))
                {
                    TempData["StatusMessage"] = $"Thêm mới chi tiết khuyến mãi (Mã KM: {ctkm.MAKM}, Mã SP: {ctkm.MASP}) thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Thêm mới chi tiết khuyến mãi thất bại. Có thể chi tiết này đã tồn tại.";
                }
            }
            ViewBag.error = ViewBag.error ?? "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại.";
            ViewBag.KHUYENMAIx = promotionDetailService.getALLPromotion();
            ViewBag.SANPHAMx = promotionDetailService.getAllProduct();
            return View(ctkm);
        }

        [HttpGet]
        public ActionResult Update(string makm, int masp)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            CHITIETKM chitietkm = promotionDetailService.getOnePromotionDetail(makm, masp);
            if (chitietkm == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy chi tiết khuyến mãi có Mã KM: {makm}, Mã SP: {masp}";
                return RedirectToAction("Index");
            }

            PromotionDetailViewModel promotionDetailViewModel = new PromotionDetailViewModel();
            promotionDetailViewModel.MAKM = chitietkm.MAKM;
            promotionDetailViewModel.MASP = chitietkm.MASP;
            promotionDetailViewModel.PHANTRAMKM = (int)chitietkm.PHANTRAMKM;
            ViewBag.KHUYENMAI = chitietkm.KHUYENMAI.TENKM;
            ViewBag.SANPHAM = chitietkm.SANPHAM.TENSP;
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.error = TempData["ErrorMessage"];
            }
            return View(promotionDetailViewModel);
        }

        [HttpPost]
        public ActionResult Update(PromotionDetailViewModel ctkm)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (ModelState.IsValid)
            {
                CHITIETKM chitietkm = new CHITIETKM();
                chitietkm.MAKM = ctkm.MAKM;
                chitietkm.MASP = ctkm.MASP;
                chitietkm.PHANTRAMKM = ctkm.PHANTRAMKM;
                if (promotionDetailService.updatePromotionDetail(chitietkm))
                {
                    TempData["StatusMessage"] = $"Cập nhật chi tiết khuyến mãi (Mã KM: {ctkm.MAKM}, Mã SP: {ctkm.MASP}) thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật chi tiết khuyến mãi thất bại. Vui lòng thử lại.";
                    return RedirectToAction("Update", new { makm = ctkm.MAKM, masp = ctkm.MASP });
                }
            }
            ViewBag.error = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại.";
            CHITIETKM chitiet = promotionDetailService.getOnePromotionDetail(ctkm.MAKM, ctkm.MASP);
            ViewBag.KHUYENMAI = chitiet.KHUYENMAI.TENKM;
            ViewBag.SANPHAM = chitiet.SANPHAM.TENSP;
            return View(ctkm);
        }

        public ActionResult Delete(string makm, int masp)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            bool result = promotionDetailService.deletePromotionDetail(makm, masp);
            if (result)
            {
                TempData["StatusMessage"] = $"Xóa chi tiết khuyến mãi (Mã KM: {makm}, Mã SP: {masp}) thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Xóa chi tiết khuyến mãi (Mã KM: {makm}, Mã SP: {masp}) thất bại. Vui lòng thử lại.";
            }
            return RedirectToAction("Index");
        }
    }
}