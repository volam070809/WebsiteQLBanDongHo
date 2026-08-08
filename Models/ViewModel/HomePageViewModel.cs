using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Areas.Admin.Models;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.ViewModel
{
    public class HomePageViewModel
    {
        public IEnumerable<SANPHAM> ProductsSelling { get; set; }
        public IEnumerable<ProductViewModel> NewProducts { get; set; }
    }
}