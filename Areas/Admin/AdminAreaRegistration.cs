using System.Web.Mvc;

namespace WebsiteQLBanDongHo.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // 1) Route cho dạng URL ngắn: /Admin/Customer (mặc định action = Index)
            //    (Một số môi trường sẽ không match đúng nếu thiếu segment {action})
            context.MapRoute(
                name: "Admin_controller_default",
                url: "Admin/{controller}",
                defaults: new { action = "Index" },
                namespaces: new[] { "WebsiteQLBanDongHo.Areas.Admin.Controllers" }
            );

            // 2) Route chuẩn: /Admin/Customer/Index, /Admin/Order/Edit/5 ...
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebsiteQLBanDongHo.Areas.Admin.Controllers" }
            );
        }
    }
}