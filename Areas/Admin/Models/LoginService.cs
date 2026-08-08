using System;
using System.Linq;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Areas.Admin.Models
{
    public class LoginService
    {
        public int Login(string name, string pass)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                var taikhoan = db.TAIKHOANs.SingleOrDefault(x => x.TENDN == name);
                if (taikhoan == null) return 0;

                if (taikhoan.TRANGTHAI == false) return -2;

                var input = pass ?? string.Empty;
                var inputMd5 = Encryptor.MD5Hash(input);
                var dbPass = taikhoan.MATKHAU ?? string.Empty;

                var passOk = string.Equals(dbPass, input, StringComparison.OrdinalIgnoreCase)
                             || string.Equals(dbPass, inputMd5, StringComparison.OrdinalIgnoreCase);

                if (!passOk) return -2;

                // Chỉ Admin mới vào được
                if (taikhoan.MALOAITK == "LK00001") return 1;
                return -1;
            }
        }

        public TAIKHOAN GetUserByName(string Name)
        {
            using (var db = new WebsiteQLBanDongHoEntities())
            {
                return db.TAIKHOANs.SingleOrDefault(x => x.TENDN == Name);
            }
        }
    }
}
