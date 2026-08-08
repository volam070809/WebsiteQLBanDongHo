using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class ProductCategoryService
    {
        WebsiteQLBanDongHoEntities db;
        public ProductCategoryService()
        {
            db = new WebsiteQLBanDongHoEntities();
        }

        public IEnumerable<LOAISANPHAM> getAllProductCategory()
        {
            return db.LOAISANPHAMs;
        }

        public bool addProductCategory(LOAISANPHAM lsp)
        {
            try
            {
                db.LOAISANPHAMs.Add(lsp);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool updateProductCategory(LOAISANPHAM lsp)
        {
            try
            {
                var result = db.LOAISANPHAMs.Find(lsp.MALOAISP);
                if (result != null)
                {
                    result.TENLOAISP = lsp.TENLOAISP;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool deleteProductCategory(string malsp)
        {
            try
            {
                string query = "DELETE FROM SANPHAM WHERE MALOAISP = '" + malsp + "'";
                string query2 = "DELETE FROM LOAISANPHAM WHERE MALOAISP = '" + malsp + "'";
                db.Database.ExecuteSqlCommand(query);
                db.Database.ExecuteSqlCommand(query2);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string getLastRecord()
        {
            string res = "";
            var lastrecord = db.LOAISANPHAMs.OrderByDescending(p => p.MALOAISP).FirstOrDefault();
            if (lastrecord != null)
            {
                res = lastrecord.MALOAISP;
            }
            return res;
        }

        public LOAISANPHAM getProductCategoryById(string malsp)
        {
            return db.LOAISANPHAMs.Find(malsp);
        }
    }
}