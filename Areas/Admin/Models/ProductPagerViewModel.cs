using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class ProductPagerViewModel
    {
        public IEnumerable<SANPHAM> Products { get; set; }
        public Pager Pager { get; set; }
    }
}