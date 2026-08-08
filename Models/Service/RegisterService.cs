using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebsiteQLBanDongHo.Common;
using WebsiteQLBanDongHo.Models.ViewModel;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class RegisterService : IRegisterSercive
    {
        public bool isExistAccount(string account)
        {
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            TAIKHOAN taikhoan = (from tk in db.TAIKHOANs where tk.TENDN.Equals(account) select tk).SingleOrDefault();
            if (taikhoan != null)
            {
                return true;
            }
            return false;
        }

        public bool isPasswordAccount(string password)
        {
            throw new NotImplementedException();
        }

        public bool isValidPassword(string password)
        {
            // Ít nhất 8 ký tự, có chữ thường, chữ hoa, số và ký tự đặc biệt
            return Regex.IsMatch(password ?? "", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
        }

        public void RegisterAccount(RegisterViewModel register)
        {
            WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();
            TAIKHOAN account = new TAIKHOAN { TENDN = register.Account, MATKHAU = Encryptor.MD5Hash(register.Password), MALOAITK = "LK00002", NGAYDANGKY = DateTime.Now, TRANGTHAI = true };
            db.TAIKHOANs.Add(account);
            db.SaveChanges();
            // MAKH là Identity, chỉ cần gán MATK đúng theo tài khoản vừa tạo
            KHACHHANG customer = new KHACHHANG
            {
                MATK = account.MATK,
                TENKH = (register.FirstName + " " + register.LastName).Trim(),
                DIACHI = register.Address,
                EMAIL = register.Email,
                SDT = register.Phone,
                GIOITINH = register.Sex
            };
            db.KHACHHANGs.Add(customer);
            db.SaveChanges();
        }

        public void RegisterAccount(Register register)
        {
            throw new NotImplementedException();
        }
    }
}