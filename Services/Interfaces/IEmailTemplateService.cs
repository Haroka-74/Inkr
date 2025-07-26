namespace Inkr.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateEmailConfirmationContent(string username, string verificationLink);
        string GenerateEmailConfirmationHtmlPage();
        string GeneratePasswordResetContent(string username, string resetLink);
        string GeneratePasswordResetForm(string userId, string token);
    }
}