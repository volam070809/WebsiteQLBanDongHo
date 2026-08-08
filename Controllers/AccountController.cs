using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.Models.Service;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Controllers
{
    public class AccountController : Controller
    {
        RegisterService registerService;
        private WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
        // GET: Account
        public ActionResult Register()
        {
            RegisterViewModel register = new RegisterViewModel();
            ViewBag.MessageRegister = "";
            return View(register);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            ViewBag.MessageRegister = "";
            registerService = new RegisterService();
            if (ModelState.IsValid)
            {
                try
                {
                    if (registerService.isExistAccount(register.Account))
                    {
                        register.Account = "";
                        ViewBag.MessageRegister += "Tài khoản đã tồn tại !";
                        return View(register);
                    }

                    if (!registerService.isValidPassword(register.Password))
                    {
                        register.Password = "";
                        ViewBag.MessageRegister += "Mật khẩu không đúng định dạng!";
                        return View(register);
                    }

                    registerService.RegisterAccount(register);

                    // Gửi mail là optional (không có SMTP config vẫn đăng ký được)
                    try
                    {
                        var contentPath = Server.MapPath("/Views/Others/newuser.html");
                        var content = System.IO.File.Exists(contentPath)
                            ? System.IO.File.ReadAllText(contentPath)
                            : "";

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var baseUrl = ConfigHelper.GetByKey(
                                "CurrentLink",
                                Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~"));
                            if (!baseUrl.EndsWith("/")) baseUrl += "/";

                            content = content.Replace("{{Account}}", register.FirstName + " " + register.LastName);
                            content = content.Replace("{{Link}}", baseUrl + "dang-nhap");
                            MailHelper.SendMail(register.Email, "Đăng ký thành công", content);
                        }
                    }
                    catch { /* ignore mail/template errors */ }

                    ViewData["SuccessMsg"] = "Đăng ký thành công";
                    register = new RegisterViewModel();
                }
                catch (Exception)
                {
                    ViewBag.MessageRegister = "Không kết nối được CSDL. Hãy kiểm tra connectionString trong Web.config (server/database) và chắc chắn SQL Server đang chạy.";
                }
            }
            return View(register);
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            ViewBag.MessageLogin = "";
            ViewBag.ReturnUrl = returnUrl;
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            ViewBag.MessageLogin = "";
            if (ModelState.IsValid)
            {
                LoginService loginService = new LoginService();
                // Truyền mật khẩu dạng plain vào service; service sẽ tự xử lý so sánh
                // (hỗ trợ cả DB đang lưu MD5 hoặc lưu plain-text do bạn tự INSERT)
                var result = loginService.Login(loginViewModel.UserName, loginViewModel.PassWord);
                if (result == 1)
                {
                    var user = loginService.GetById(loginViewModel.UserName);

                    var userSession = new UserLogin
                    {
                        UserName = user.TENDN,
                        UserID = user.MATK
                    };

                    // Xác định quyền: Admin hay User
                    var isAdmin = IsAdminAccount(user);

                    // Set session theo quyền
                    Session[CommonConstands.USER_SESSION] = userSession;
                    if (isAdmin)
                    {
                        Session[CommonConstands.ADMIN_SESSION] = userSession;
                    }
                    else
                    {
                        // đảm bảo user thường không bị "dính" session admin
                        Session[CommonConstands.ADMIN_SESSION] = null;
                    }

                    // Điều hướng theo quyền
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        // User thường không được quay về link /Admin
                        if (!isAdmin && returnUrl.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            return Redirect("/");
                        }
                        return Redirect(returnUrl);
                    }

                    return isAdmin ? Redirect("~/Admin/Home/Index") : Redirect("/");
                }
                else if (result == 0)
                {
                    ViewBag.MessageLogin += "Tài khoản không tồn tại";
                }
                else if (result == -1)
                {
                    ViewBag.MessageLogin += "Tài khoản đang bị khóa";
                }
                else if (result == -2)
                {
                    ViewBag.MessageLogin += "Mật khẩu không đúng";
                }
                else
                {
                    ViewBag.MessageLogin += "Đăng nhập không thành công";
                }
            }
            return View(loginViewModel);
        }

        private static bool IsAdminAccount(TAIKHOAN user)
        {
            if (user == null) return false;

            // Mặc định theo data seed của dự án: LK00001 = Admin
            if (string.Equals(user.MALOAITK, "LK00001", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Fallback: dựa vào tên loại tài khoản nếu có
            try
            {
                var name = user.LOAITK?.TENLOAITK ?? string.Empty;
                return name.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public ActionResult Profile(string returnUrl)
        {
            var session = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("Profile", "Account", new { returnUrl }) });
            }

            var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == session.UserID);
            if (kh == null)
            {
                kh = new KHACHHANG { MATK = (int)session.UserID };
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
            }

            var vm = new UserProfileViewModel
            {
                UserName = session.UserName,
                TENKH = kh.TENKH,
                EMAIL = kh.EMAIL,
                SDT = kh.SDT,
                GIOITINH = kh.GIOITINH,
                DIACHI = kh.DIACHI,
                ReturnUrl = returnUrl
            };

            // Chỉ cho phép 2 giá trị giới tính: Nam/Nữ
            if (vm.GIOITINH != "Nam" && vm.GIOITINH != "Nữ")
            {
                vm.GIOITINH = "Nam";
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(UserProfileViewModel model)
        {
            var session = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("Profile", "Account", new { returnUrl = model?.ReturnUrl }) });
            }

            // Chỉ cho phép 2 giá trị giới tính: Nam/Nữ
            if (model.GIOITINH != "Nam" && model.GIOITINH != "Nữ")
            {
                ModelState.AddModelError("GIOITINH", "Giới tính chỉ được chọn Nam hoặc Nữ");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == session.UserID);
            if (kh == null)
            {
                kh = new KHACHHANG { MATK = (int)session.UserID };
                db.KHACHHANGs.Add(kh);
            }

            kh.TENKH = model.TENKH;
            kh.EMAIL = model.EMAIL;
            kh.SDT = model.SDT;
            kh.GIOITINH = model.GIOITINH;
            kh.DIACHI = model.DIACHI;
            db.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin cá nhân thành công.";
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return RedirectToAction("Profile");
        }

        public ActionResult Orders()
        {
            var session = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("Orders", "Account") });
            }

            var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == session.UserID);
            if (kh == null)
            {
                TempData["Error"] = "Bạn cần cập nhật thông tin cá nhân trước.";
                return RedirectToAction("Profile", new { returnUrl = Url.Action("Orders", "Account") });
            }

            var orders = db.DONHANGs
                .Where(o => o.MAKH == kh.MAKH)
                .OrderByDescending(o => o.NGAYDAT)
                .ToList();

            ViewBag.UserName = session.UserName;
            return View(orders);
        }

        public ActionResult OrderDetail(int id)
        {
            var session = (UserLogin)Session[CommonConstands.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("OrderDetail", "Account", new { id }) });
            }

            var kh = db.KHACHHANGs.FirstOrDefault(x => x.MATK == session.UserID);
            if (kh == null)
            {
                TempData["Error"] = "Bạn cần cập nhật thông tin cá nhân trước.";
                return RedirectToAction("Profile", new { returnUrl = Url.Action("OrderDetail", "Account", new { id }) });
            }

            var order = db.DONHANGs.Include("CHITIETDONHANGs.SANPHAM")
                .FirstOrDefault(o => o.MADH == id && o.MAKH == kh.MAKH);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Orders");
            }

            return View(order);
        }

        public ActionResult Logout()
        {
            // logout sạch: xoá cả user + admin session
            Session[CommonConstands.USER_SESSION] = null;
            Session[CommonConstands.ADMIN_SESSION] = null;
            return Redirect("/");
        }

        [ChildActionOnly]
        public ActionResult UserMenu()
        {
            return PartialView();
        }
    }
}