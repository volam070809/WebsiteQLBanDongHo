using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebsiteQLBanDongHo.Common
{
    public class MailHelper
    {
        public static bool SendMail(string toEmail, string subject, string content)
        {
            try
            {
                // Nếu chưa cấu hình SMTP trong Web.config thì bỏ qua (tránh crash khi đăng ký)
                var host = ConfigHelper.GetByKey("SMTPHost");
                var portStr = ConfigHelper.GetByKey("SMTPPort");
                var fromEmail = ConfigHelper.GetByKey("FromEmailAddress");
                var password = ConfigHelper.GetByKey("FromEmailPassword");
                var fromName = ConfigHelper.GetByKey("FromName", "WATCH LUXURY");

                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(password))
                    return false;

                int port = 0;
                if (!int.TryParse(portStr, out port)) port = 587;

                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };
                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };
                mail.To.Add(new MailAddress(toEmail));
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                smtpClient.Send(mail);
                return true;
            }
            catch (SmtpException smex)
            {
                Console.WriteLine("SMTP Error: " + smex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail Error: " + ex.Message);
                return false;
            }
        }
    }
}