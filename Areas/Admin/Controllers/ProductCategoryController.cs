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
    public class ProductCategoryController : Controller
    {
        ProductCategoryService productCategoryService = new ProductCategoryService();

        [HttpGet]
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            if (TempData["StatusMessage"] != null)
            {
                ViewBag.message = TempData["StatusMessage"];
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.error = TempData["ErrorMessage"];
            }
            return View(productCategoryService.getAllProductCategory());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel();
            productCategoryViewModel.MALOAISP = newMALOAISP(productCategoryService.getLastRecord());
            return View(productCategoryViewModel);
        }

        [HttpPost]
        public ActionResult Create(ProductCategoryViewModel lsp)
        {
            if (ModelState.IsValid)
            {
                LOAISANPHAM loaisp = new LOAISANPHAM();
                loaisp.MALOAISP = lsp.MALOAISP;
                loaisp.TENLOAISP = lsp.TENLOAISP;
                if (productCategoryService.addProductCategory(loaisp))
                {
                    TempData["StatusMessage"] = $"Thêm mới loại sản phẩm {lsp.TENLOAISP} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "Thêm mới loại sản phẩm thất bại";
                }
            }
            ViewBag.message = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại";
            return View(lsp);
        }

        [HttpGet]
        public ActionResult Update(string malsp)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel();
            var res = productCategoryService.getProductCategoryById(malsp);
            if (res == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy loại sản phẩm này";
                return RedirectToAction("Index");
            }

            productCategoryViewModel.MALOAISP = res.MALOAISP;
            productCategoryViewModel.TENLOAISP = res.TENLOAISP;

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.message = TempData["ErrorMessage"];
            }
            return View(productCategoryViewModel);
        }

        [HttpPost]
        public ActionResult Update(ProductCategoryViewModel lsp)
        {
            if (ModelState.IsValid)
            {
                LOAISANPHAM loaisp = new LOAISANPHAM();
                loaisp.MALOAISP = lsp.MALOAISP;
                loaisp.TENLOAISP = lsp.TENLOAISP;

                if (productCategoryService.updateProductCategory(loaisp))
                {
                    TempData["StatusMessage"] = $"Cập nhật loại sản phẩm {lsp.TENLOAISP} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật loại sản phẩm thất bại. Vui lòng thử lại";
                    return RedirectToAction("Update", new { malsp = lsp.MALOAISP });
                }
            }
            ViewBag.message = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại";
            return View(lsp);
        }

        public ActionResult Delete(string malsp)
        {
            bool result = productCategoryService.deleteProductCategory(malsp);
            if (result)
            {
                TempData["StatusMessage"] = $"Xóa loại sản phẩm {malsp} thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Xóa loại sản phẩm {malsp} thất bại. Có thể loại sản phẩm đang được sử dụng";
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public string newMALOAISP(string lastMALOAISP)
        {
            string res = "LP00001";
            if (String.Compare(lastMALOAISP, "", false) != 0)
            {
                int tam = Int32.Parse(lastMALOAISP.Substring(2)) + 1;
                string rs = tam.ToString();
                while (rs.Length < 5)
                {
                    rs = "0" + rs;
                }
                res = "LP" + rs;
            }
            return res;
        }
    }
}