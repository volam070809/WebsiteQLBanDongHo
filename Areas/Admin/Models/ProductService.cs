using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class ProductService
    {
        WebsiteQLBanDongHoEntities db;
        public ProductService()
        {
            db = new WebsiteQLBanDongHoEntities();
        }

        public IEnumerable<SANPHAM> getAllProduct()
        {
            return db.SANPHAMs;
        }

        public int getTotalRecord(string keyword = "", int? brandId = null)
        {
            var query = db.SANPHAMs.AsQueryable();
            string normalizedKeyword = keyword?.ToLower().Trim();
            if (!string.IsNullOrEmpty(normalizedKeyword))
            {
                if (int.TryParse(normalizedKeyword, out int maspId))
                {
                    query = query.Where(sp => sp.MASP == maspId);
                }
                else
                {
                    query = query.Where(sp => sp.TENSP != null && sp.TENSP.ToLower().Contains(normalizedKeyword));
                }
            }
            if (brandId.HasValue && brandId.Value > 0)
            {
                query = query.Where(sp => sp.MATH == brandId.Value);
            }
            return query.Count();
        }

        public SANPHAM getProductById(int masp)
        {
            return db.SANPHAMs.Find(masp);
        }

        public bool addProduct(SANPHAM sp)
        {
            try
            {
                db.SANPHAMs.Add(sp);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool updateProduct(SANPHAM sp)
        {
            try
            {
                var result = db.SANPHAMs.Find(sp.MASP);
                if (result != null)
                {
                    result.TENSP = sp.TENSP;
                    result.SOLUONG = sp.SOLUONG;
                    result.MATH = sp.MATH;
                    result.MOTA = sp.MOTA;
                    result.DONGIA = sp.DONGIA;
                    result.MALOAISP = sp.MALOAISP;
                    result.HINHLON = sp.HINHLON;
                    result.HINHNHO = sp.HINHNHO;
                    result.DANHGIA = sp.DANHGIA;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool deleteProduct(int masp)
        {
            try
            {
                string query = "DELETE FROM CHITIETKM WHERE MASP = '" + masp + "'";
                string query2 = "DELETE FROM CHITIETDONHANG WHERE MASP = '" + masp + "'";
                string query3 = "DELETE FROM SANPHAM WHERE MASP = '" + masp + "'";
                db.Database.ExecuteSqlCommand(query);
                db.Database.ExecuteSqlCommand(query2);
                db.Database.ExecuteSqlCommand(query3);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SANPHAM> loadProduct(int pageIndex, int pageSize, string keyword = "", int? brandId = null)
        {
            var query = db.SANPHAMs.AsQueryable();
            string normalizedKeyword = keyword?.ToLower().Trim();
            if (!string.IsNullOrEmpty(normalizedKeyword))
            {
                if (int.TryParse(normalizedKeyword, out int maspId))
                {
                    query = query.Where(sp => sp.MASP == maspId);
                }
                else
                {
                    query = query.Where(sp => sp.TENSP != null && sp.TENSP.ToLower().Contains(normalizedKeyword));
                }
            }
            if (brandId.HasValue && brandId.Value > 0)
            {
                query = query.Where(sp => sp.MATH == brandId.Value);
            }
            var ListProduct = query.OrderBy(sp => sp.MASP).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return ListProduct.ToList();
        }

        public IEnumerable<LOAISANPHAM> getLoaiSanPham()
        {
            return db.LOAISANPHAMs;
        }

        public IEnumerable<THUONGHIEU> getThuongHieu()
        {
            return db.THUONGHIEUx;
        }
    }
}