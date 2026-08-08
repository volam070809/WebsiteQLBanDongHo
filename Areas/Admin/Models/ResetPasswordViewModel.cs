using System.ComponentModel.DataAnnotations;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class ResetPasswordViewModel
    {
        public int MATK { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Mật khẩu tối thiểu 4 ký tự")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
