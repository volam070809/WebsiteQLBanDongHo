using System;
using System.Linq;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private readonly WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        // GET: Admin/Report/SoldReport
        public ActionResult SoldReport()
        {
            var session = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (session == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            // Loại trừ các đơn đã hủy (admin đang dùng các string trạng thái theo dữ liệu thực tế)
            // Lưu ý: DONGIA trong DB là double? (float) nên cần tính doanh thu dạng double trước,
            // sau đó convert sang decimal để hiển thị.
            var raw = db.CHITIETDONHANGs
                .Where(ct => ct.DONHANG != null &&
                             (ct.DONHANG.TRANGTHAI == null || !ct.DONHANG.TRANGTHAI.ToLower().Contains("hủy")))
                .GroupBy(ct => new { ct.MASP, TENSP = ct.SANPHAM.TENSP })
                .Select(g => new
                {
                    MASP = g.Key.MASP,
                    TENSP = g.Key.TENSP,
                    SO_LUONG_BAN = g.Sum(x => (int?)(x.SOLUONG) ?? 0),
                    DOANH_THU_DOUBLE = g.Sum(x => (x.SANPHAM.DONGIA ?? 0) * (double)((int?)(x.SOLUONG) ?? 0))
                })
                .OrderByDescending(x => x.SO_LUONG_BAN)
                .Take(50)
                .ToList();

            var rows = raw
                .Select(x => new SoldProductReportRow
                {
                    MASP = x.MASP,
                    TENSP = x.TENSP,
                    SO_LUONG_BAN = x.SO_LUONG_BAN,
                    DOANH_THU = (decimal)x.DOANH_THU_DOUBLE
                })
                .ToList();

            return View(rows);
        }
    }
}
