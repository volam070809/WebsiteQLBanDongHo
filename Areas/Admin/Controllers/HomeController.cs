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
    public class HomeController : Controller
    {
        private DashboardService dashboardService = new DashboardService();
        private WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
            ViewBag.UserCount = dashboardService.GetUserCount();
            ViewBag.TotalRevenue = dashboardService.GetTotalRevenue();
            ViewBag.OrderCount = dashboardService.GetOrderCount();
            ViewBag.ProductCount = dashboardService.GetProductCount();
            ViewBag.CustomerCount = dashboardService.GetCustomerCount();
            ViewBag.SoldProductCount = dashboardService.GetSoldProductCount();
            ViewBag.CommentCount = dashboardService.GetCommentCount();
            ViewBag.ActivePromotionCount = dashboardService.GetActivePromotionCount();
            return View();
        }

        public ActionResult Logout()
        {
            // logout sạch: xoá cả admin + user session (tránh còn dính đăng nhập ở giao diện user)
            Session[CommonConstands.ADMIN_SESSION] = null;
            Session[CommonConstands.USER_SESSION] = null;
            return Redirect("~/dang-nhap");
        }

        [HttpGet]
        public JsonResult GetRevenueData()
        {
            var currentYear = DateTime.Now.Year;
            var revenueData = db.DONHANGs
                .Where(o => o.NGAYDAT.HasValue && o.NGAYDAT.Value.Year == currentYear && o.TRANGTHAI == "Đã giao")
                .GroupBy(o => o.NGAYDAT.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.TONGTIEN ?? 0)
                })
                .ToList();

            var monthlyData = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Month = m,
                    Total = revenueData.FirstOrDefault(x => x.Month == m)?.Total ?? 0
                })
                .OrderBy(x => x.Month);
            return Json(monthlyData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrderStatusData()
        {
            var orderData = db.DONHANGs
                .Where(o => !string.IsNullOrEmpty(o.TRANGTHAI))
                .GroupBy(o => o.TRANGTHAI)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();
            return Json(orderData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTopSellingProducts()
        {
            var topProducts = db.CHITIETDONHANGs
                .Where(ct => ct.DONHANG != null && ct.DONHANG.TRANGTHAI == "Đã giao")
                .GroupBy(c => c.MASP)
                .Select(g => new
                {
                    ProductName = g.FirstOrDefault().SANPHAM.TENSP,
                    TotalSold = g.Sum(x => (int?)x.SOLUONG) ?? 0
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();
            return Json(topProducts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNewUsersPerMonth()
        {
            var currentYear = DateTime.Now.Year;
            var userData = db.TAIKHOANs
                .Where(u => u.NGAYDANGKY.HasValue && u.NGAYDANGKY.Value.Year == currentYear)
                .GroupBy(u => u.NGAYDANGKY.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var monthlyData = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Month = m,
                    Count = userData.FirstOrDefault(x => x.Month == m)?.Count ?? 0
                })
                .OrderBy(x => x.Month);
            return Json(monthlyData, JsonRequestBehavior.AllowGet);
        }
    }
}