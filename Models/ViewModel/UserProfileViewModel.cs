using System.ComponentModel.DataAnnotations;

namespace WebsiteQLBanDongHo.Models.ViewModel
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập họ tên")]
        public string TENKH { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "*Email không hợp lệ")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(\d{4,15})$", ErrorMessage = "*Số điện thoại không hợp lệ")]
        public string SDT { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [System.ComponentModel.DataAnnotations.RegularExpression("^(Nam|Nữ)$", ErrorMessage = "Giới tính chỉ được chọn Nam hoặc Nữ")]
        public string GIOITINH { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập địa chỉ")]
        public string DIACHI { get; set; }

        public string ReturnUrl { get; set; }
    }
}
