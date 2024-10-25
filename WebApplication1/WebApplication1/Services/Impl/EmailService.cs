using System.Net.Mail;
using System.Net;
using WebApplication1.Core.Utils;

namespace WebApplication1.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var decryptedUsername = EncryptionHelper.Decrypt(Environment.GetEnvironmentVariable("SMTP_USERNAME"));
            var decryptedPassword = EncryptionHelper.Decrypt(Environment.GetEnvironmentVariable("SMTP_PASSWORD"));


            var smtpClient = new SmtpClient
            {
                Host = smtpSettings["Host"],
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSSL"]),
                //Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
                Credentials = new NetworkCredential(decryptedUsername, decryptedPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Username"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
