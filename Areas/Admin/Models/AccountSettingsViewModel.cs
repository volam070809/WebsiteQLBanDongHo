using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class AccountSettingsViewModel
    {
        public int MATK { get; set; }
        public string TENDN { get; set; }
        public string TENKH { get; set; }
        public string EMAIL { get; set; }
        public string SDT { get; set; }
        public string GIOITINH { get; set; }
        public string DIACHI { get; set; }
        public DateTime? NGAYDANGKY { get; set; }
        public bool? TRANGTHAI { get; set; }
        public string LOAITK { get; set; }
    }
}