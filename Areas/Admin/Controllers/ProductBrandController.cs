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
    public class ProductBrandController : Controller
    {
        ProductBrandService productBrandService = new ProductBrandService();
        // GET: Admin/ProductBrand
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
            return View(productBrandService.getAllProductBrand());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }
            ProductBrandViewModel th = new ProductBrandViewModel();
            return View(th);
        }

        [HttpPost]
        public ActionResult Create(ProductBrandViewModel th)
        {
            if (ModelState.IsValid)
            {
                THUONGHIEU thuonghieu = new THUONGHIEU();
                thuonghieu.TENTH = th.TENTH;
                thuonghieu.HINHTH = th.HINHTH;

                if (productBrandService.addProductBrand(thuonghieu))
                {
                    TempData["StatusMessage"] = $"Thêm mới thương hiệu {th.TENTH} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Thêm mới thương hiệu thất bại. Có thể tên thương hiệu đã tồn tại";
                }
            }
            ViewBag.error = ViewBag.error ?? "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại";
            return View(th);
        }

        [HttpGet]
        public ActionResult Update(int math)
        {
            var userSession = (UserLogin)Session[CommonConstands.ADMIN_SESSION];
            if (userSession == null)
            {
                return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl));
            }

            THUONGHIEU thuonghieu = productBrandService.getProductBrandById(math);
            if (thuonghieu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thương hiệu này";
                return RedirectToAction("Index");
            }

            ProductBrandViewModel th = new ProductBrandViewModel();
            th.MATH = math;
            th.TENTH = thuonghieu.TENTH;
            th.HINHTH = thuonghieu.HINHTH;
            ViewBag.MATH = thuonghieu.HINHTH;
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.error = TempData["ErrorMessage"];
            }
            return View(th);
        }

        [HttpPost]
        public ActionResult Update(ProductBrandViewModel th)
        {
            if (ModelState.IsValid)
            {
                THUONGHIEU thuonghieu = new THUONGHIEU();
                thuonghieu.MATH = th.MATH;
                thuonghieu.TENTH = th.TENTH;
                thuonghieu.HINHTH = th.HINHTH;

                if (productBrandService.updateProductBrand(thuonghieu))
                {
                    TempData["StatusMessage"] = $"Cập nhật thương hiệu {th.TENTH} thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật thương hiệu thất bại. Vui lòng thử lại";
                    return RedirectToAction("Update", new { math = th.MATH });
                }
            }
            ViewBag.error = "Dữ liệu nhập không hợp lệ. Vui lòng kiểm tra lại";
            return View(th);
        }

        public ActionResult Delete(int math)
        {
            bool result = productBrandService.deleteProductBrand(math);
            if (result)
            {
                TempData["StatusMessage"] = $"Xóa thương hiệu có mã {math} thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Xóa thương hiệu có mã {math} thất bại. Có thể thương hiệu đang được sử dụng";
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PhotoCreate()
        {
            bool result = false;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = System.IO.Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/HINHTH/"), fileName);
                    file.SaveAs(path);
                    result = true;
                    break;
                }
            }
            return Json(new { result = result });
        }
    }
}