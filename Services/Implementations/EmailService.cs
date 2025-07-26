using Inkr.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Inkr.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(AppSettings.Email.Smtp.Sender));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(AppSettings.Email.Smtp.Host, AppSettings.Email.Smtp.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(AppSettings.Email.Smtp.Username, AppSettings.Email.Smtp.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}