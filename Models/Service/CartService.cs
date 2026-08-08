using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class CartService
    {
        public static bool CheckNumberProduct(int id, int soluong)
        {
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            if (soluong < 1) soluong = 1;
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(s => s.MASP == id);
            if (sp == null) return false;
            return (sp.SOLUONG ?? 0) >= soluong;
        }
    }
}