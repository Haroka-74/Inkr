using Inkr.Models;

namespace Inkr.Services.Interfaces
{
    public interface IAuthNotificationService
    {
        Task SendEmailConfirmationAsync(InkrUser user, string confirmationLink);
        Task SendPasswordResetAsync(InkrUser user, string resetLink);
    }
}