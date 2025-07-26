using Inkr.DTOs.Auth;
using Inkr.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inkr.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IEmailTemplateService emailTemplateService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var result = await authService.RegisterAsync(request);

            if (result.IsSuccess)
                return StatusCode(result.StatusCode, result.Data);

            return StatusCode(result.StatusCode, new { error = result.Error });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var result = await authService.LoginAsync(request);

            if (result.IsSuccess)
                return StatusCode(result.StatusCode, result.Data);

            return StatusCode(result.StatusCode, new { error = result.Error });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO refreshToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var result = await authService.RefreshTokenAsync(refreshToken);

            if (result.IsSuccess)
                return StatusCode(result.StatusCode, result.Data);

            return StatusCode(result.StatusCode, new { error = result.Error });
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync(RefreshTokenDTO refreshToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            await authService.RevokeTokenAsync(refreshToken);

            return NoContent();
        }

        [HttpGet("profile"), Authorize]
        public IActionResult GetProfileAsync()
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized(new { error = "Missing or invalid Authorization header" });

            var token = authorizationHeader["Bearer ".Length..].Trim();

            var result = authService.GetProfileAsync(token);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { error = result.Error });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPut("profile"), Authorize]
        public async Task<IActionResult> UpdateProfileAsync(UpdateProfileRequestDTO request)
        {
            var result = await authService.UpdateProfileAsync(request);

            if (result.IsSuccess)
                return StatusCode(result.StatusCode, result.Data);

            return StatusCode(result.StatusCode, new { error = result.Error });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await authService.ConfirmEmailAsync(userId, token);

            if(!result.IsSuccess)
                return StatusCode(result.StatusCode, new { error = result.Error });

            return Content(result.Data!, "text/html");
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            await authService.ResendEmailConfirmationAsync(request);

            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            await authService.ForgotPasswordAsync(request);

            return NoContent();
        }

        [HttpGet("reset-password-form")]
        public IActionResult ResetPasswordForm([FromQuery] string userId, [FromQuery] string token)
        {
            return Content(emailTemplateService.GeneratePasswordResetForm(userId, token), "text/html");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var result = await authService.ResetPasswordAsync(request);

            if (result.IsSuccess)
                return StatusCode(result.StatusCode, new { data = result.Data });

            return StatusCode(result.StatusCode, new { error = result.Error });
        }
    }
}