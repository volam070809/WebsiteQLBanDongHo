using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class DashboardService
    {
        private WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
        public int GetUserCount()
        {
            return db.TAIKHOANs.Count();
        }

        public double GetTotalRevenue()
        {
            return db.DONHANGs.Where(x => x.TRANGTHAI == "Đã giao").Sum(x => (double?)x.TONGTIEN) ?? 0;
        }

        public int GetOrderCount()
        {
            return db.DONHANGs.Count();
        }

        public int GetProductCount()
        {
            return db.SANPHAMs.Count();
        }

        public int GetCustomerCount()
        {
            return db.KHACHHANGs.Count();
        }

        public int GetSoldProductCount()
        {
            // Chỉ tính các sản phẩm đã bán thực tế (đơn hàng đã giao)
            return (from ct in db.CHITIETDONHANGs
                    join dh in db.DONHANGs on ct.MADH equals dh.MADH
                    where dh.TRANGTHAI == "Đã giao"
                    select ct.SOLUONG).Sum(x => (int?)x) ?? 0;
        }

        public int GetCommentCount()
        {
            return db.BINHLUANs.Count();
        }

        public int GetActivePromotionCount()
        {
            DateTime now = DateTime.Now;
            return db.KHUYENMAIs.Where(km => km.NGAYBD <= now && km.NGAYKT >= now).Count();
        }
    }
}