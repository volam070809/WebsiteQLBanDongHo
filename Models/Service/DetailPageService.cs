using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class DetailPageService
    {
        public static SANPHAM LoadDetailProduct(int Id)
        {
            SANPHAM res = null;
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            res = db.SANPHAMs.Find(Id);
            return res;
        }

        public static IEnumerable<SANPHAM> LoadListProductRelative(int Id)
        {
            SANPHAM product = LoadDetailProduct(Id);
            if (product == null)
            {
                List<SANPHAM> lstListNewProduct = new List<SANPHAM>();
                foreach (ProductViewModel sp in ProductService.GetListNewProducts().Take(3))
                {
                    lstListNewProduct.Add(sp.Product);
                }
                return lstListNewProduct;
            }

            IEnumerable<SANPHAM> res = null;
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            res = (from sp in db.SANPHAMs where sp.MATH == product.MATH select sp);
            if (res != null && res.ToList().Count < 3)
            {
                int amount = res.ToList().Count;
                List<SANPHAM> lsp = new List<SANPHAM>();
                foreach (var item in res)
                {
                    lsp.Add(item);
                }
                foreach (var item in ProductService.GetListNewProducts().Take(3 - amount))
                {
                    lsp.Add(item.Product);
                }
                return lsp;
            }
            return res;
        }

        public static string GetTag(int Id)
        {
            string res = "";
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            int trademark = db.SANPHAMs.Find(Id).MATH.Value;
            res = db.THUONGHIEUx.Find(trademark).TENTH;
            return res;
        }
    }
}