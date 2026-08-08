using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductService productService = new ProductService();
        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            ViewBag.BrandList = productService.getThuongHieu();
            var pager = new Pager(productService.getTotalRecord(), page);
            var viewModel = new ProductPagerViewModel
            {
                Products = productService.loadProduct(pager.CurrentPage, pager.PageSize),
                Pager = pager
            };
            return View(viewModel);
        }

        public ActionResult Product(int page = 1, string keyword = "", int? brandId = null)
        {
            string searchKeyword = keyword ?? "";
            int pageSize = 5;
            var totalRecord = productService.getTotalRecord(searchKeyword, brandId);
            var pager = new Pager(totalRecord, page, pageSize);
            var filteredProducts = productService.loadProduct(pager.CurrentPage, pageSize, searchKeyword, brandId);
            var viewModel = new ProductPagerViewModel
            {
                Products = filteredProducts,
                Pager = pager
            };
            return PartialView("Product", viewModel);
        }

        public ActionResult Create()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            ProductViewModel productviewmodel = new ProductViewModel();
            ViewBag.ThuongHieu = productService.getThuongHieu();
            ViewBag.LoaiSanPham = productService.getLoaiSanPham();
            return View(productviewmodel);
        }

        public ActionResult Detail(int masp)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            return View(productService.getProductById(masp));
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel sanpham)
        {
            ViewBag.message = "";
            ViewBag.ThuongHieu = productService.getThuongHieu();
            ViewBag.LoaiSanPham = productService.getLoaiSanPham();
            if (ModelState.IsValid)
            {
                SANPHAM sp = new SANPHAM();
                sp.TENSP = sanpham.TENSP;
                sp.SOLUONG = sanpham.SOLUONG;
                sp.MATH = sanpham.MATH;
                sp.MOTA = sanpham.MOTA;
                sp.DANHGIA = sanpham.DANHGIA;
                sp.DONGIA = sanpham.DONGIA;
                sp.MALOAISP = sanpham.MALOAISP;
                sp.HINHLON = sanpham.HINHLON;
                sp.HINHNHO = sanpham.HINHLON.Split('.')[0];
                if (productService.addProduct(sp))
                {
                    ViewBag.message = "Thêm mới sản phẩm thành công";
                    sanpham = new ProductViewModel();
                    return View(sanpham);
                }
                return View(sanpham);
            }
            return View(sanpham);
        }

        [HttpGet]
        public ActionResult Update(int masp)
        {
            ViewBag.ThuongHieu = productService.getThuongHieu();
            ViewBag.LoaiSanPham = productService.getLoaiSanPham();
            ProductViewModel sp = new ProductViewModel();
            var sanpham = productService.getProductById(masp);
            sp.MASP = sanpham.MASP;
            sp.TENSP = sanpham.TENSP;
            sp.SOLUONG = (int)sanpham.SOLUONG;
            sp.MATH = (int)sanpham.MATH;
            sp.MOTA = sanpham.MOTA;
            sp.DANHGIA = sanpham.DANHGIA;
            sp.DONGIA = (double)sanpham.DONGIA;
            sp.MALOAISP = sanpham.MALOAISP;
            sp.HINHLON = sanpham.HINHLON;
            sp.HINHNHO = sanpham.HINHNHO;
            return View(sp);
        }

        [HttpPost]
        public ActionResult Update(ProductViewModel sanpham)
        {
            ViewBag.ThuongHieu = productService.getThuongHieu();
            ViewBag.LoaiSanPham = productService.getLoaiSanPham();
            ViewBag.message = "";
            if (ModelState.IsValid)
            {
                SANPHAM sp = new SANPHAM();
                sp.MASP = sanpham.MASP;
                sp.TENSP = sanpham.TENSP;
                sp.SOLUONG = (int)sanpham.SOLUONG;
                sp.MATH = (int)sanpham.MATH;
                sp.MOTA = sanpham.MOTA;
                sp.DANHGIA = sanpham.DANHGIA;
                sp.DONGIA = (double)sanpham.DONGIA;
                sp.MALOAISP = sanpham.MALOAISP;
                sp.HINHLON = sanpham.HINHLON;
                sp.HINHNHO = sanpham.HINHNHO;
                if (productService.updateProduct(sp))
                {
                    TempData["message"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                ViewBag.message = "Cập nhật thất bại!";
                return View(sanpham);
            }
            return View(sanpham);
        }

        public ActionResult Delete(int masp)
        {
            return Json(new { result = productService.deleteProduct(masp) });
        }

        [HttpPost]
        public JsonResult PhotoCreate(string tenhinh)
        {
            bool result = false;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null)
                {
                    var path = Path.Combine(Server.MapPath("~/images/HINHLON/"), tenhinh);
                    file.SaveAs(path);
                    result = true;
                }
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult Photo(string hinhlon, string hinhnho)
        {
            bool result = false;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var FolderUploadDir = Server.MapPath("~/images/HINHNHO/" + hinhlon);
                if (file != null)
                {
                    if (!Directory.Exists(FolderUploadDir))
                    {
                        Directory.CreateDirectory(FolderUploadDir);
                    }
                    var path = Path.Combine(Server.MapPath("~/images/HINHNHO/" + hinhlon + "/"), hinhnho);
                    file.SaveAs(path);
                    result = true;
                }
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult PhotoUpdate(string hinhlon, string hinhnho)
        {
            bool result = false;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null)
                {
                    var path = Path.Combine(Server.MapPath("~/images/HINHNHO/" + hinhlon + "/"), hinhnho);
                    file.SaveAs(path);
                    result = true;
                }
            }
            return Json(new { result = result });
        }
    }
}