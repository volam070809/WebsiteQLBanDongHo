using System.Web.Mvc;

namespace WebsiteQLBanDongHo.Controllers
{
    /// <summary>
    /// Controller này tồn tại để tương thích với các link cũ dạng /Order/OrderHistory.
    /// Thực tế lịch sử đơn hàng được xử lý bởi AccountController (action Orders).
    /// </summary>
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult OrderHistory()
        {
            // Redirect về action đúng để tránh 404.
            return RedirectToAction("Orders", "Account");
        }
    }
}
