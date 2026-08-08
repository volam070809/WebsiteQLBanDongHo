using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class PromotionViewModel
    {
        [Required]
        public string MAKM { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khuyến mãi")]
        public string TENKM { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tháng bắt đầu")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày bắt đầu không đúng định dạng ngày tháng.")]
        public DateTime NGAYBD { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tháng kết thúc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày kết thúc không đúng định dạng ngày tháng.")]

        public DateTime NGAYKT { get; set; }
    }
}