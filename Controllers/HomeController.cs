using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Models.Service;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Controllers
{
    public class HomeController : Controller
    {
        private WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        public HomeController() { }
        // GET: Home
        public ActionResult Index()
        {
            HomePageViewModel HomePageVM = new HomePageViewModel();
            HomePageVM.ProductsSelling = ProductService.GetListProductsSelling();
            HomePageVM.NewProducts = ProductService.GetListNewProducts().Take(8);
            return View(HomePageVM);
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Detail(int? id = null)
        {
            if (id == null)
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm để xem chi tiết.";
                return RedirectToAction("Index");
            }

            DetailViewModel detailPage = new DetailViewModel();
            detailPage.Product = new ProductViewModel();
            detailPage.Product.Product = ProductService.Find(id.Value);
            if (detailPage.Product.Product == null)
            {
                TempData["Error"] = $"Không tìm thấy sản phẩm có mã {id.Value}.";
                return RedirectToAction("Index");
            }

            detailPage.Product.Promotion = PromotionService.GetPromotion(id.Value);
            if (detailPage.Product.Product.MATH.HasValue)
            {
                detailPage.ListProductsRelative = ProductService.GetListProductRelative(detailPage.Product.Product.MATH.Value).Take(3);
            }
            else
            {
                detailPage.ListProductsRelative = new List<ProductViewModel>();
            }
            detailPage.Tag = DetailPageService.GetTag(id.Value);
            detailPage.ListNewProducts = ProductService.GetListNewProducts().Take(8);
            return View(detailPage);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if (ContactService.SendMail(contact))
                {
                    TempData["Success"] = "Tin nhắn của bạn đã được gửi thành công. Chúng tôi sẽ phản hồi sớm nhất!";
                    return RedirectToAction("Contact");
                }
                else
                {
                    ModelState.AddModelError("", "Gửi email thất bại, vui lòng kiểm tra lại thông tin và thử lại.");
                }
            }
            return View(contact);
        }

        public ActionResult Account()
        {
            return View();
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