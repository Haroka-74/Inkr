using Inkr.Services.Interfaces;

namespace Inkr.Services.Implementations
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateEmailConfirmationContent(string username, string verificationLink)
        {
            return
            $$"""
                <h2> Welcome to Inkr! </h2>
                <p> Hi {{username}}, </p>
                <p> To activate your account, please confirm your email by clicking the button below: </p>
                <a style = "display:inline-block; padding:10px 20px; background:#4CAF50; color:white; text-decoration:none;" href = "{{verificationLink}}"> Verify Email </a>
                <p> If you did not register, please ignore this message. </p>
            """;
        }

        public string GenerateEmailConfirmationHtmlPage()
        {
            return
            $$"""
                <!DOCTYPE html>
                <html>
                <head>
                    <title> Email Confirmed - Inkr </title>
                </head>
                <body>
                    <center>
                        <h1> Email Verified Successfully! </h1>
                        <p> Your Inkr account has been activated. </p>                    
                    </center>
                </body>
                </html>
            """;
        }

        public string GeneratePasswordResetContent(string username, string resetLink)
        {
            return
            $$"""
                <h2> Reset Your Inkr Password </h2>
                <p> Hi {{username}}, </p>
                <p> We received a request to reset your password. Click the button below to create a new password: </p>
                <a style = "display:inline-block; padding:10px 20px; background:#FF6B6B; color:white; text-decoration:none;" href = "{{resetLink}}"> Reset Password </a>
                <p> If you did not request this password reset, please ignore this message. </p>
                <p> For security reasons, this link will expire in 1 hour. </p>
            """;
        }

        public string GeneratePasswordResetForm(string userId, string token)
        {
            return
            $$"""
            <!DOCTYPE html>
            <html>
            <head>
                <title> Reset Password </title>
                <style>
                    body { font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 20px; }
                    .form-group { margin-bottom: 15px; }
                    label { display: block; margin-bottom: 5px; }
                    input[type='password'] { width: 100%; padding: 8px; }
                    button { padding: 10px 15px; background: #4CAF50; color: white; border: none; }
                    .message { margin-top: 15px; padding: 10px; display: none; }
                    .success { background: #dff0d8; color: #3c763d; }
                    .error { background: #f2dede; color: #a94442; }
                </style>
            </head>
            <body>
                <h2> Reset Your Password </h2>
                <form id = 'resetForm'>
                    <input type = 'hidden' id = 'userId' value = '{{userId}}'>
                    <input type = 'hidden' id = 'token' value = '{{token}}'>
                    <div class='form-group'>
                        <label for = 'newPassword'> New Password: </label>
                        <input type = 'password' id = 'newPassword' required pattern = '(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}' title = 'Must contain at least one number, one uppercase and lowercase letter, and at least 8 characters'>
                    </div>
                    <div class = 'form-group'>
                        <label for = 'confirmPassword'> Confirm Password: </label>
                        <input type = 'password' id = 'confirmPassword' required>
                    </div>
                    <button type='submit'> Reset Password </button>
                </form>
                <div id = 'message' class = 'message'></div>
                <script>
                    document.getElementById('resetForm').addEventListener('submit', async function(e) {
                        e.preventDefault();

                        const userId = document.getElementById('userId').value;
                        const token = document.getElementById('token').value;
                        const newPassword = document.getElementById('newPassword').value;
                        const confirmPassword = document.getElementById('confirmPassword').value;
                        const messageEl = document.getElementById('message');

                        messageEl.style.display = 'none';
                        messageEl.className = 'message';

                        if (newPassword !== confirmPassword) {
                            messageEl.textContent = 'Passwords do not match!';
                            messageEl.classList.add('error');
                            messageEl.style.display = 'block';
                            return;
                        }

                        try {
                            const response = await fetch('/api/auth/reset-password', {
                                method: 'POST',
                                headers: {
                                    'Content-Type': 'application/json',
                                },
                                body: JSON.stringify({
                                    userId: userId,
                                    token: token,
                                    newPassword: newPassword
                                })
                            });

                            const result = await response.json();

                            if (response.ok) {
                                messageEl.textContent = 'Password reset successfully!';
                                messageEl.classList.add('success');
                            } else {
                                messageEl.textContent = result.error || 'Error resetting password';
                                messageEl.classList.add('error');
                            }
                            messageEl.style.display = 'block';
                        } catch (error) {
                            messageEl.textContent = 'Network error occurred';
                            messageEl.classList.add('error');
                            messageEl.style.display = 'block';
                        }
                    });
                </script>
            </body>
            </html>
            """;
        }
    }
}