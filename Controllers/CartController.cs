using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Models.Models;
using WebsiteQLBanDongHo.Models.Service;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
            }
            Session["Cart"] = cart;
            return View(Session["Cart"] as Cart);
        }

        public ActionResult AddProduct(int id, int sl = 1)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
            }

            if (sl < 1) sl = 1;
            cart.AddProduct(id, sl);
            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string DeleteProduct(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                return "Tổng Cộng: 0 VNĐ";
            }
            cart.RemoveProduct(id);
            Session["Cart"] = cart;
            return "Tổng Cộng: " + cart.TotalMoney().ToString("0,0") + " VNĐ";
        }

        [HttpPost]
        public JsonResult UpdateProduct(int id, int sl)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Giỏ hàng trống.",
                    appliedQuantity = 0,
                    totalText = "Tổng Cộng: 0 VNĐ"
                });
            }

            if (sl < 1) sl = 1;

            // Kiểm tra tồn kho để cảnh báo người dùng nếu vượt quá
            int stock = 0;
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var sp = db.SANPHAMs.Find(id);
                stock = sp?.SOLUONG ?? 0;
            }

            int applied = sl;
            string message = null;
            bool ok = true;

            if (stock <= 0)
            {
                ok = false;
                applied = 0;
                message = "Sản phẩm đã hết hàng.";
                cart.RemoveProduct(id);
            }
            else if (sl > stock)
            {
                ok = false;
                applied = stock;
                message = $"Số lượng trong kho không đủ. Chỉ còn {stock} sản phẩm.";
                cart.UpdateProduct(id, applied);
            }
            else
            {
                cart.UpdateProduct(id, sl);
            }

            Session["Cart"] = cart;
            string totalText = "Tổng Cộng: " + cart.TotalMoney().ToString("0,0") + " VNĐ";

            return Json(new
            {
                success = ok,
                message,
                appliedQuantity = applied,
                totalText
            });
        }

        public ActionResult Checkout()
        {
            // Bắt buộc có giỏ hàng
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || cart.GetList().Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }

            // Kiểm tra tồn kho
            foreach (var item in cart.GetList())
            {
                if (!CartService.CheckNumberProduct(item.Product.MASP, item.Quantity))
                {
                    TempData["Error"] = "Một số sản phẩm trong giỏ hàng không đủ số lượng.";
                    return RedirectToAction("Index");
                }
            }

            // Bắt buộc đăng nhập trước khi thanh toán
            var userSession = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Cart") });
            }

            // Bắt buộc đủ thông tin khách hàng
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == userSession.UserID);
                bool thieuThongTin = (kh == null)
                                   || string.IsNullOrWhiteSpace(kh.TENKH)
                                   || string.IsNullOrWhiteSpace(kh.EMAIL)
                                   || string.IsNullOrWhiteSpace(kh.SDT)
                                   || string.IsNullOrWhiteSpace(kh.DIACHI);

                if (thieuThongTin)
                {
                    TempData["Error"] = "Bạn cần cập nhật đầy đủ thông tin cá nhân trước khi thanh toán.";
                    return RedirectToAction("Profile", "Account", new { returnUrl = Url.Action("Index", "CusInfo") });
                }
            }

            return RedirectToAction("Index", "CusInfo");
        }

        [ChildActionOnly]
        public ActionResult CartMenu()
        {
            return PartialView();
        }
    }
}