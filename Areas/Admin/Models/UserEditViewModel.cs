using System.ComponentModel.DataAnnotations;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class UserEditViewModel
    {
        public int MATK { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string TENDN { get; set; }

        [Display(Name = "Quyền")]
        [Required(ErrorMessage = "Vui lòng chọn quyền")]
        public string MALOAITK { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TRANGTHAI { get; set; }
    }
}
