using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net.Mail;
using MailKit.Security;


namespace HairSalon.Services.Service
{

    public static class MailService
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            
            smtp.Authenticate("dathlecnx4@gmail.com", "cnsbobsikneiqtuw");
            var mailMessage = new MailMessage
            {
                From = new MailAddress("dathlecnx4@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            try
            {
                smtp.Send((MimeKit.MimeMessage)mailMessage);
                smtp.Disconnect(true);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}


