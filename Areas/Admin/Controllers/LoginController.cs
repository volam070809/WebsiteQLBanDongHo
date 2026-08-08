using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.Common;

namespace WebsiteQLBanDongHo.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        // GET: Admin/Login
        public ActionResult Login()
        {
            // Dùng chung 1 trang đăng nhập: /dang-nhap
            return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode("/Admin/Home/Index"));
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            // Dùng chung 1 trang đăng nhập: /dang-nhap
            return Redirect("~/dang-nhap?returnUrl=" + System.Web.HttpUtility.UrlEncode("/Admin/Home/Index"));
        }
    }
}