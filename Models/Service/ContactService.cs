using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebsiteQLBanDongHo.Models.Service
{
    public class ContactService
    {
        public static bool SendMail(ViewModel.ContactViewModel model)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("nguyenhaithanhptbt@gmail.com", "Mã_App_Password_16_ký_tự_vừa_tạo");
                String message = "Mail send from " + model.Name + "\n" + "Phone: " + model.Phone + "\n" + model.Content;
                smtp.Send(model.Email, "nguyenhaithanhptbt@gmail.com", "Contact from " + model.Name, message);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LỖI GỬI EMAIL: " + ex.Message);
                return false;
            }
        }
    }
}