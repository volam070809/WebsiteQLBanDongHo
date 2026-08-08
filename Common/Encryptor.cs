using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebsiteQLBanDongHo.Common
{
    public class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            // Tính toán hàm băm từ byte văn bản
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            // Nhận được kết quả băm sau khi tính toán nó
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                // Thay đổi nó thành 2 chữ số thập lục phân cho mỗi byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}