using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class OrderService
    {
        WebsiteQLBanDongHoEntities db;
        public OrderService()
        {
            db = new WebsiteQLBanDongHoEntities();
        }

        public IEnumerable<DONHANG> getAllOrder()
        {
            return db.DONHANGs.OrderByDescending(n => n.MADH);
        }

        public DONHANG GetOrderByID(int madh)
        {
            return db.DONHANGs.FirstOrDefault(n => n.MADH == madh);
        }

        public List<CHITIETDONHANG> LoadOrderDetail(int mahd)
        {
            return db.CHITIETDONHANGs.Where(n => n.MADH == mahd).ToList();
        }

        public List<SANPHAM> LoadAllProduct()
        {
            return db.SANPHAMs.ToList();
        }

        public List<KHACHHANG> LoadAllCustomer()
        {
            return db.KHACHHANGs.ToList();
        }

        public int? TakeQuantityProduct(int masp)
        {
            return db.SANPHAMs.Where(n => n.MASP == masp).FirstOrDefault().SOLUONG;
        }

        public SANPHAM GetProductByID(int masp)
        {
            return db.SANPHAMs.Where(n => n.MASP == masp).FirstOrDefault();
        }

        public int GetNextOrderID()
        {
            var maxId = db.DONHANGs.Any() ? db.DONHANGs.Max(d => d.MADH) : 0;
            return maxId + 1;
        }

        public int InsertOrder(DONHANG donhang)
        {
            try
            {
                donhang.MADH = GetNextOrderID();
                db.DONHANGs.Add(donhang);
                db.SaveChanges();
                return donhang.MADH;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LỖI DB KHI INSERT ĐƠN HÀNG: {ex.Message}");
                return 0;
            }
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                var detailsToDelete = db.CHITIETDONHANGs.Where(ct => ct.MADH == id).ToList();
                db.CHITIETDONHANGs.RemoveRange(detailsToDelete);
                var order = db.DONHANGs.Find(id);
                if (order != null)
                {
                    db.DONHANGs.Remove(order);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LỖI DB KHI XÓA ĐƠN HÀNG {id}: {ex.Message}");
                return false;
            }
        }

        public void InsertDetailOrder(int mahd, int masp, int soluong)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                CHITIETDONHANG chitietdonhang = new CHITIETDONHANG { MADH = mahd, MASP = masp, SOLUONG = soluong };
                try
                {
                    dbContext.CHITIETDONHANGs.Add(chitietdonhang);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void UpdateOrderTotal(int madh, double? totalMoney)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                var donhang = dbContext.DONHANGs.Find(madh);
                if (donhang != null)
                {
                    donhang.TONGTIEN = totalMoney;
                    dbContext.DONHANGs.AddOrUpdate(donhang);
                    dbContext.SaveChanges();
                }
            }
        }

        public int HandleQuantityProduct(int masp, int soluong, bool kiemtra)
        {
            int? soluongtonkho = GetProductByID(masp)?.SOLUONG;
            if (!soluongtonkho.HasValue) soluongtonkho = 0;
            if (kiemtra)
            {
                soluongtonkho += soluong;
            }
            else
            {
                soluongtonkho -= soluong;
            }
            return soluongtonkho.Value;
        }

        public void UpdateQuantityProduct(int masp, int soluong, bool kiemtra)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                var sanphamCanCapNhat = dbContext.SANPHAMs.Where(n => n.MASP == masp).FirstOrDefault();
                if (sanphamCanCapNhat != null)
                {
                    int? soluonghientai = sanphamCanCapNhat.SOLUONG ?? 0;
                    if (kiemtra)
                    {
                        sanphamCanCapNhat.SOLUONG = soluonghientai + soluong;
                    }
                    else
                    {
                        sanphamCanCapNhat.SOLUONG = soluonghientai - soluong;
                    }
                    try
                    {
                        dbContext.SANPHAMs.AddOrUpdate(sanphamCanCapNhat);
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void UpdateCustomer(OrderViewModel orderViewModel)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                var result = dbContext.KHACHHANGs.Where(n => n.MAKH == orderViewModel.makh).FirstOrDefault();
                if (result != null)
                {
                    try
                    {
                        result.DIACHI = orderViewModel.diachi;
                        result.TENKH = orderViewModel.tennguoinhan;
                        result.SDT = orderViewModel.sodt;
                        dbContext.KHACHHANGs.AddOrUpdate(result);
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void UpdateOrder(OrderViewModel orderViewModel, double? totalMoney)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                var result = dbContext.DONHANGs.Where(n => n.MADH == orderViewModel.mahd).FirstOrDefault();
                if (result != null)
                {
                    try
                    {
                        result.TRANGTHAI = orderViewModel.tinhtrang;
                        result.DIACHIGIAO = orderViewModel.diachi;
                        result.SDT = orderViewModel.sodt;
                        result.NGAYGIAO = orderViewModel.ngaygiao;
                        result.TONGTIEN = totalMoney;
                        dbContext.DONHANGs.AddOrUpdate(result);
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void DeleteDetailOrder(OrderViewModel orderViewModel)
        {
            using (var dbContext = new WebsiteQLBanDongHoEntities())
            {
                var result = dbContext.CHITIETDONHANGs.Where(n => n.MADH == orderViewModel.mahd);
                if (result != null)
                {
                    try
                    {
                        var detailsToDelete = result.ToList();
                        foreach (var item in detailsToDelete)
                        {
                            dbContext.CHITIETDONHANGs.Remove(item);
                        }
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}