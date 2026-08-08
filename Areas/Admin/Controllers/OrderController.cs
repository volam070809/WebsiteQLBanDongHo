using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        OrderService orderService = new OrderService();
        // GET: Admin/Order
        public ActionResult Index()
        {
            if (TempData["IndexMessage"] != null)
            {
                ViewBag.message = TempData["IndexMessage"];
            }
            if (TempData["IndexError"] != null)
            {
                ViewBag.error = TempData["IndexError"];
            }
            return View();
        }

        public ActionResult TakeListOrderLimit(string search, int page = 1)
        {
            int pageSize = 6;
            using (var db = new WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext.WebsiteQLBanDongHoEntities())
            {
                var orders = db.DONHANGs.Include("KHACHHANG").AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    string keyword = search.Trim().ToLower();
                    int maDhSearch;
                    bool isNumeric = int.TryParse(search.Trim(), out maDhSearch);
                    orders = orders.Where(o =>
                        (isNumeric && o.MADH == maDhSearch) ||
                        (!isNumeric && (
                            o.MADH.ToString().Contains(keyword) ||
                            (o.SDT != null && o.SDT.Contains(keyword)) ||
                            (o.TRANGTHAI != null && o.TRANGTHAI.ToLower().Contains(keyword)) ||
                            (o.KHACHHANG != null && o.KHACHHANG.TENKH.ToLower().Contains(keyword))
                        ))
                    );
                }
                orders = orders.OrderByDescending(o => o.NGAYDAT);

                int totalItems = orders.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;
                var pagedOrders = orders.Skip((page - 1) * pageSize).Take(pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.SearchKeyword = search;
                return PartialView(pagedOrders.ToList());
            }
        }

        public ActionResult LoadOrderDetail(int mahd)
        {
            List<CHITIETDONHANG> listctdh = orderService.LoadOrderDetail(mahd);
            return View(listctdh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrder(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldDetails = orderService.LoadOrderDetail(orderViewModel.mahd);
                    foreach (var detail in oldDetails)
                    {
                        orderService.UpdateQuantityProduct(detail.MASP, detail.SOLUONG ?? 0, true);
                    }
                    orderService.DeleteDetailOrder(orderViewModel);
                    double? totalMoney = 0;
                    if (orderViewModel.MaspUpdate != null && orderViewModel.SoluongUpdate != null && orderViewModel.MaspUpdate.Length == orderViewModel.SoluongUpdate.Length)
                    {
                        for (int i = 0; i < orderViewModel.MaspUpdate.Length; i++)
                        {
                            int masp = orderViewModel.MaspUpdate[i];
                            int soluong = orderViewModel.SoluongUpdate[i];
                            if (soluong <= 0) continue;
                            orderService.InsertDetailOrder(orderViewModel.mahd, masp, soluong);
                            var product = orderService.GetProductByID(masp);
                            if (product != null && product.DONGIA.HasValue)
                            {
                                totalMoney += soluong * product.DONGIA.Value;
                                orderService.UpdateQuantityProduct(masp, soluong, false);
                            }
                        }
                    }
                    orderService.UpdateCustomer(orderViewModel);
                    orderService.UpdateOrder(orderViewModel, totalMoney);
                    return Json(new { success = true, message = "Cập nhật đơn hàng #" + orderViewModel.mahd + " thành công." });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật đơn hàng {orderViewModel.mahd}: {ex.Message}");
                    return Json(new { success = false, message = "Lỗi khi cập nhật đơn hàng: " + ex.Message });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            string errorMsg = string.Join(" | ", errors.Where(s => !string.IsNullOrEmpty(s)));
            return Json(new { success = false, message = "Dữ liệu gửi lên không hợp lệ: " + (string.IsNullOrEmpty(errorMsg) ? "Vui lòng kiểm tra các trường bắt buộc." : errorMsg) });
        }

        [HttpGet]
        public ActionResult CreateOrder()
        {
            ViewBag.MAKH = new SelectList(orderService.LoadAllCustomer(), "MAKH", "TENKH");
            ViewBag.MASP = new SelectList(orderService.LoadAllProduct(), "MASP", "TENSP");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(DONHANG donhang, int[] mangmasp, int[] mangsoluong)
        {
            if (donhang.MAKH == 0)
            {
                ModelState.AddModelError("MAKH", "Vui lòng chọn khách hàng.");
            }

            bool isProductArrayValid = (mangmasp != null && mangsoluong != null && mangmasp.Length > 0);
            if (!isProductArrayValid)
            {
                ModelState.AddModelError("", "Đơn hàng phải có ít nhất một sản phẩm.");
            }

            if (ModelState.IsValid && isProductArrayValid)
            {
                if (donhang.NGAYDAT == null)
                {
                    donhang.NGAYDAT = DateTime.Now;
                }
                if (string.IsNullOrEmpty(donhang.TRANGTHAI))
                {
                    donhang.TRANGTHAI = "chờ kiểm duyệt";
                }
                donhang.TONGTIEN = 0;

                int newMadH = orderService.InsertOrder(donhang);
                if (newMadH > 0)
                {
                    double? totalMoney = 0;
                    try
                    {
                        if (mangmasp.Length == mangsoluong.Length)
                        {
                            for (int i = 0; i < mangmasp.Length; i++)
                            {
                                int masp = mangmasp[i];
                                int soluong = mangsoluong[i];
                                if (soluong <= 0) continue;

                                orderService.InsertDetailOrder(newMadH, masp, soluong);
                                SANPHAM sanpham = orderService.GetProductByID(masp);
                                if (sanpham != null && sanpham.DONGIA.HasValue)
                                {
                                    totalMoney += soluong * sanpham.DONGIA.Value;
                                    orderService.UpdateQuantityProduct(masp, soluong, false);
                                }
                            }
                        }
                        orderService.UpdateOrderTotal(newMadH, totalMoney);
                        TempData["IndexMessage"] = "Thêm đơn hàng #" + newMadH + " thành công!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"LỖI CHI TIẾT ĐƠN HÀNG, ĐANG ROLLBACK ĐH #{newMadH}: {ex.Message}");
                        orderService.DeleteOrder(newMadH);
                        ModelState.AddModelError("", "Lỗi xử lý chi tiết đơn hàng (Đã hủy đơn hàng). Chi tiết: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi khi thêm đơn hàng vào CSDL.");
                }
            }
            ViewBag.MAKH = new SelectList(orderService.LoadAllCustomer(), "MAKH", "TENKH", donhang.MAKH);
            ViewBag.MASP = new SelectList(orderService.LoadAllProduct(), "MASP", "TENSP");
            return View(donhang);
        }

        [HttpPost]
        public JsonResult DeleteOrder(int id)
        {
            try
            {
                var order = orderService.GetOrderByID(id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng cần xóa." });
                }
                if (!order.TRANGTHAI.Equals("đã hủy", StringComparison.OrdinalIgnoreCase))
                {
                    var details = orderService.LoadOrderDetail(id);
                    if (details != null)
                    {
                        foreach (var detail in details)
                        {
                            orderService.UpdateQuantityProduct(detail.MASP, detail.SOLUONG ?? 0, true);
                        }
                    }
                }
                bool result = orderService.DeleteOrder(id);
                if (result)
                {
                    return Json(new { success = true, message = "Đơn hàng #" + id + " đã được xóa/hủy thành công." });
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi logic Service: Hàm xóa đơn hàng trả về thất bại (false). Kiểm tra logic xóa chi tiết đơn hàng." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LỖI TRUY CẬP DB/LOGIC DỮ LIỆU ĐƠN HÀNG {id}: {ex.Message}");
                return Json(new { success = false, message = "Lỗi hệ thống khi xóa đơn hàng. Nguyên nhân: " + ex.Message });
            }
        }
    }
}