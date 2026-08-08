using System;
using System.Linq;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class LoginService
    {
        private readonly WebsiteQLBanDongHoEntities db;

        public LoginService()
        {
            db = new WebsiteQLBanDongHoEntities();
        }

        public TAIKHOAN GetById(string userName)
        {
            return db.TAIKHOANs.SingleOrDefault(n => n.TENDN == userName);
        }

        /// <summary>
        /// Hỗ trợ cả 2 kiểu lưu mật khẩu trong DB:
        /// - Lưu MD5 (đúng theo code đăng ký)
        /// - Lưu plain text (trường hợp bạn tự INSERT bằng tay trong SQL)
        /// </summary>
        public int Login(string userName, string passWord)
        {
            var result = db.TAIKHOANs.SingleOrDefault(n => n.TENDN == userName);
            if (result == null) return 0;
            if (result.TRANGTHAI == false) return -1;

            var input = passWord ?? string.Empty;
            var inputMd5 = Encryptor.MD5Hash(input);
            var dbPass = result.MATKHAU ?? string.Empty;

            if (string.Equals(dbPass, input, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(dbPass, inputMd5, StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }

            return -2;
        }
    }
}
