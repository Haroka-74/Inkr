using Inkr.Models;
using Inkr.Services.Interfaces;

namespace Inkr.Services.Implementations
{
    public class AuthNotificationService(IEmailService emailService, IEmailTemplateService emailTemplateService) : IAuthNotificationService
    {
        public async Task SendEmailConfirmationAsync(InkrUser user, string confirmationLink)
        {
            var emailBody = emailTemplateService.GenerateEmailConfirmationContent(user.UserName!, confirmationLink);
            await emailService.SendAsync(user.Email!, "Verify your Inkr email", emailBody);
        }

        public async Task SendPasswordResetAsync(InkrUser user, string resetLink)
        {
            var emailBody = emailTemplateService.GeneratePasswordResetContent(user.UserName!, resetLink);
            await emailService.SendAsync(user.Email!, "Reset your Inkr password", emailBody);
        }
    }
}