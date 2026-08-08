using System;
using System.Data.Entity;
using System.Linq;
using WebsiteQLBanDongHo.Models.Models;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class CusInfoService
    {
        public static bool CheckNumberProduct(int id, int sl)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                if (sl < 1) sl = 1;
                var sp = db.SANPHAMs.SingleOrDefault(s => s.MASP == id);
                if (sp == null) return false;
                return (sp.SOLUONG ?? 0) >= sl;
            }
        }

        /// <summary>
        /// Lấy MAKH theo MATK. Nếu chưa có KHACHHANG thì tự tạo mới.
        /// </summary>
        public static int GetOrCreateCustomerId(long accountId)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == accountId);
                if (kh != null) return kh.MAKH;

                var newKh = new KHACHHANG
                {
                    MATK = (int)accountId,
                    TENKH = "",
                    EMAIL = "",
                    SDT = "",
                    DIACHI = "",
                    GIOITINH = ""
                };
                db.KHACHHANGs.Add(newKh);
                db.SaveChanges();
                return newKh.MAKH;
            }
        }

        /// <summary>
        /// Tạo DONHANG + CHITIETDONHANG, trừ tồn kho, trạng thái mặc định: "chờ kiểm duyệt".
        /// </summary>
        public static int AddBill(CusInfoViewModel model, int idKhachHang)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.cart == null || model.cart.GetList().Count == 0)
                throw new InvalidOperationException("Giỏ hàng trống.");

            using (var db = new WebsiteQLBanDongHoEntities())
            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    string paymentLabel;
                    switch ((model.PaymentMethod ?? "").Trim().ToUpperInvariant())
                    {
                        case "BANK_QR":
                        case "BANK":
                        case "QR":
                            paymentLabel = "Chuyển khoản / QR";
                            break;
                        case "MOMO":
                            paymentLabel = "MoMo";
                            break;
                        case "ZALOPAY":
                            paymentLabel = "ZaloPay";
                            break;
                        default:
                            paymentLabel = "COD";
                            break;
                    }

                    var mota = $"Thanh toán: {paymentLabel}";
                    if (!string.IsNullOrWhiteSpace(model.MoTa))
                    {
                        mota += $" | Ghi chú: {model.MoTa.Trim()}";
                    }

                    // MADH không Identity => tự tăng
                    int nextMadH = (db.DONHANGs.Any() ? db.DONHANGs.Max(d => d.MADH) : 0) + 1;

                    var donhang = new DONHANG
                    {
                        MADH = nextMadH,
                        MAKH = idKhachHang,
                        DIACHIGIAO = model.DiaChiGiao,
                        SDT = model.Sdt,
                        MOTA = mota,
                        TONGTIEN = model.cart.TotalMoney(),
                        TRANGTHAI = "chờ kiểm duyệt",
                        NGAYDAT = DateTime.Now,
                        NGAYGIAO = DateTime.Now.AddDays(7)
                    };

                    db.DONHANGs.Add(donhang);

                    // Tạo chi tiết + trừ tồn kho
                    foreach (var item in model.cart.GetList())
                    {
                        var sp = db.SANPHAMs.SingleOrDefault(s => s.MASP == item.Product.MASP);
                        if (sp == null)
                            throw new InvalidOperationException("Sản phẩm không tồn tại.");

                        int buyQty = item.Quantity;
                        int stock = sp.SOLUONG ?? 0;
                        if (buyQty < 1) buyQty = 1;
                        if (stock < buyQty)
                            throw new InvalidOperationException($"Sản phẩm '{sp.TENSP}' không đủ số lượng.");

                        db.CHITIETDONHANGs.Add(new CHITIETDONHANG
                        {
                            MADH = nextMadH,
                            MASP = sp.MASP,
                            SOLUONG = buyQty
                        });

                        sp.SOLUONG = stock - buyQty;
                        db.Entry(sp).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    tx.Commit();
                    return nextMadH;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
