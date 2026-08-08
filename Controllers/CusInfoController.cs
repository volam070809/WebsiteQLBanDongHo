using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.Models.Models;
using WebsiteQLBanDongHo.Models.Service;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Controllers
{
    public class CusInfoController : Controller
    {
        private bool IsProfileComplete(long userId)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == userId);
                return kh != null
                    && !string.IsNullOrWhiteSpace(kh.TENKH)
                    && !string.IsNullOrWhiteSpace(kh.EMAIL)
                    && !string.IsNullOrWhiteSpace(kh.SDT)
                    && !string.IsNullOrWhiteSpace(kh.DIACHI);
            }
        }

        [HttpGet]
        // GET: CusInfo
        public ActionResult Index()
        {
            // Bắt buộc đăng nhập
            var userSession = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "CusInfo") });
            }

            // Bắt buộc có giỏ hàng
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new Cart();
            }
            var cart = Session["Cart"] as Cart;
            if (cart == null || cart.GetList().Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            // Bắt buộc đủ thông tin cá nhân
            if (!IsProfileComplete(userSession.UserID))
            {
                TempData["Error"] = "Bạn cần cập nhật đầy đủ thông tin cá nhân trước khi thanh toán.";
                return RedirectToAction("Profile", "Account", new { returnUrl = Url.Action("Index", "CusInfo") });
            }

            // Prefill từ thông tin khách hàng
            var vm = new CusInfoViewModel { cart = cart, PaymentMethod = "COD" };
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == userSession.UserID);
                if (kh != null)
                {
                    vm.DiaChiGiao = kh.DIACHI;
                    vm.Sdt = kh.SDT;
                }
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CusInfoViewModel model)
        {
            var userSession = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "CusInfo") });
            }

            if (Session["Cart"] == null)
            {
                Session["Cart"] = new Cart();
            }
            model.cart = Session["Cart"] as Cart;

            if (model.cart == null || model.cart.GetList().Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            if (!IsProfileComplete(userSession.UserID))
            {
                TempData["Error"] = "Bạn cần cập nhật đầy đủ thông tin cá nhân trước khi thanh toán.";
                return RedirectToAction("Profile", "Account", new { returnUrl = Url.Action("Index", "CusInfo") });
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check tồn kho lần cuối
            foreach (var item in model.cart.GetList())
            {
                if (!CusInfoService.CheckNumberProduct(item.Product.MASP, item.Quantity))
                {
                    TempData["Error"] = "Một số sản phẩm không đủ số lượng. Vui lòng kiểm tra lại giỏ hàng.";
                    return RedirectToAction("Index", "Cart");
                }
            }

            // Tạo đơn hàng
            int idKh = CusInfoService.GetOrCreateCustomerId(userSession.UserID);
            CusInfoService.AddBill(model, idKh);

            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            Session["Cart"] = null;
            return View();
        }
    }
}