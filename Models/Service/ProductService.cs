using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class ProductService
    {
        public static List<SANPHAM> GetListProductsSelling()
        {
            List<SANPHAM> ListProductsSelling = null;
            int Month = DateTime.Today.Month;
            int Year = DateTime.Today.Year;
            if (DateTime.Today.Day < 15)
            {
                if (Month - 1 == 0)
                {
                    Year -= 1;
                    Month = 12;
                }
                else
                {
                    Month -= 1;
                }
            }

            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            try
            {
                ListProductsSelling = (from sp in db.SANPHAMs let totalQuantity = (from ct in db.CHITIETDONHANGs join dh in db.DONHANGs on ct.MADH equals dh.MADH where sp.MASP == ct.MASP && dh.NGAYDAT.Value.Month == Month && dh.NGAYDAT.Value.Year == Year select ct.SOLUONG).Sum() where totalQuantity > 0 orderby totalQuantity descending select sp).Take(3).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm bán chạy: " + e.Message);
            }

            if (ListProductsSelling == null || ListProductsSelling.ToList().Count < 3)
            {
                List<SANPHAM> lstListNewProduct = new List<SANPHAM>();
                foreach (ProductViewModel sp in GetListNewProducts().Take(3))
                {
                    lstListNewProduct.Add(sp.Product);
                }
                return lstListNewProduct;
            }
            List<SANPHAM> lsp = ListProductsSelling.ToList();
            return ListProductsSelling;
        }

        internal static List<ProductViewModel> GetListProductRelative(int idTrademark)
        {
            List<ProductViewModel> result = new List<ProductViewModel>();
            List<SANPHAM> ListProducts = new List<SANPHAM>();
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                ListProducts = (from sp in db.SANPHAMs where sp.MATH == idTrademark orderby sp.MASP descending select sp).ToList();
            }
            foreach (SANPHAM sp in ListProducts)
            {
                int Promotion = PromotionService.GetPromotion(sp.MASP);
                ProductViewModel productViewModel = new ProductViewModel { Product = sp, Promotion = Promotion };
                result.Add(productViewModel);
            }
            return result;
        }

        public static List<ProductViewModel> GetListNewProducts()
        {
            List<ProductViewModel> result = new List<ProductViewModel>();
            List<SANPHAM> ListNewProducts = null;
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                ListNewProducts = (from sp in db.SANPHAMs orderby sp.MASP descending select sp).ToList();
            }
            foreach (SANPHAM sp in ListNewProducts)
            {
                int Promotion = PromotionService.GetPromotion(sp.MASP);
                ProductViewModel productViewModel = new ProductViewModel { Product = sp, Promotion = Promotion };
                result.Add(productViewModel);
            }
            return result;
        }

        public static SANPHAM Find(int id)
        {
            SANPHAM result = null;
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                result = db.SANPHAMs.Find(id);
            }
            return result;
        }

        public static List<string> ListName(string keyword)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                return db.SANPHAMs.Where(n => n.TENSP.Contains(keyword)).Select(n => n.TENSP).ToList();
            }
        }

        public static List<ProductViewModel> Search(string keyword)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                List<ProductViewModel> result = new List<ProductViewModel>();
                List<SANPHAM> listProduct = new List<SANPHAM>();
                listProduct = (from sp in db.SANPHAMs where sp.TENSP.Contains(keyword) select sp).ToList();
                foreach (SANPHAM sp in listProduct)
                {
                    int Promotion = PromotionService.GetPromotion(sp.MASP);
                    ProductViewModel productViewModel = new ProductViewModel { Product = sp, Promotion = Promotion };
                    result.Add(productViewModel);
                }
                return result;
            }
        }
    }
}