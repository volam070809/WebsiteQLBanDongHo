using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;
using WebsiteQLBanDongHo.Models.Service;

namespace WebsiteQLBanDongHo.Models.Models
{
    public class Cart
    {
        private List<CartItem> Products = new List<CartItem>();
        public List<string> Message
        {
            get
            {
                List<string> res = new List<string>();
                foreach (var item in Products)
                {
                    if (!CartService.CheckNumberProduct(item.Product.MASP, item.Quantity))
                    {
                        string mes = "Sản phẩm " + item.Product.TENSP + " không đủ số lượng";
                        res.Add(mes);
                    }
                }
                return res;
            }
        }

        public void AddProduct(int id, int soluong)
        {
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            var sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
                return;

            if (soluong < 1) soluong = 1;

            // Clamp số lượng theo tồn kho để tránh vượt quá số lượng thực tế
            int stock = sanpham.SOLUONG ?? 0;
            if (stock <= 0) return;
            soluong = Math.Min(soluong, stock);

            var item = (from i in Products where i.Product.MASP == sanpham.MASP select i).SingleOrDefault();
            if (item != null)
            {
                item.Quantity = Math.Min(item.Quantity + soluong, stock);
            }
            else
            {
                int Promotion = PromotionService.GetPromotion(id);
                Products.Add(new CartItem { Product = sanpham, Quantity = soluong, Promotion = Promotion });
            }
        }

        public void RemoveProduct(int id)
        {
            var item = Products.SingleOrDefault(i => i.Product != null && i.Product.MASP == id);
            if (item != null)
            {
                Products.Remove(item);
            }
        }

        public void UpdateProduct(int id, int soluong)
        {
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            // Tìm sản phẩm từ CSDL
            var sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
                return;

            if (soluong <= 0)
            {
                RemoveProduct(id);
                return;
            }

            int stock = sanpham.SOLUONG ?? 0;
            soluong = Math.Min(soluong, stock);

            var item = (from i in Products where i.Product.MASP == sanpham.MASP select i).SingleOrDefault();
            if (item != null)
            {
                item.Quantity = soluong;
            }
        }

        public List<CartItem> GetList()
        {
            return Products;
        }

        public double TotalMoney()
        {
            if (Products.Count == 0)
            {
                return 0;
            }

            return Products
                .Where(pi => pi.Product != null && pi.Product.DONGIA.HasValue)
                .Sum(pi => pi.Quantity * (pi.Product.DONGIA.Value * (100.0 - pi.Promotion) / 100.0));
        }
    }
}