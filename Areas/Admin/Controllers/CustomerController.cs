using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        // GET: Admin/Customer
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            var customers = db.KHACHHANGs
                .Include(x => x.TAIKHOAN)
                .OrderByDescending(x => x.MAKH)
                .ToList();

            return View(customers);
        }
    }
}
