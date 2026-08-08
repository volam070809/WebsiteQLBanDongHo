using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class OrderViewModel
    {
        public List<DONHANG> ListOrder { get; set; }
        public List<CHITIETDONHANG> ListOrderDetail { get; set; }
        public List<SANPHAM> ListProduct { get; set; }
        public int TotalPage { get; set; }
        public int mahd { get; set; }
        public DateTime? ngaymua { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ngaygiao { get; set; }
        public String tinhtrang { get; set; }
        public int makh { get; set; }
        public String tennguoinhan { get; set; }
        public String sodt { get; set; }
        public String diachi { get; set; }
        public int[] MaspUpdate { get; set; }
        public int[] SoluongUpdate { get; set; }
        public String[] Mangtensp { get; set; }
    }
}