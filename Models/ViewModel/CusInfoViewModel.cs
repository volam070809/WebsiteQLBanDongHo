using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Models.Models;

namespace WebsiteQLBanDongHo.Models.ViewModel
{
    public class CusInfoViewModel
    {
        [Required(ErrorMessage = "*Vui lòng nhập địa chỉ giao hàng")]
        public string DiaChiGiao { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "*Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(\d{4,15})$", ErrorMessage = "*{0} không hợp lệ")]
        public string Sdt { get; set; }

        public string MoTa { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "*Vui lòng chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        public Cart cart { get; set; }
    }
}