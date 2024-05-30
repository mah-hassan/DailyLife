using DailyLife.Api.Contracts;
using DailyLife.Application.Identity.Commands.ConfirmEmail;
using DailyLife.Application.Identity.Commands.DeleteProfilePicture;
using DailyLife.Application.Identity.Commands.ForgetPassword;
using DailyLife.Application.Identity.Commands.Login;
using DailyLife.Application.Identity.Commands.Register;
using DailyLife.Application.Identity.Commands.ResetPassword;
using DailyLife.Application.Identity.Commands.SetProfilePicture;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyLife.Api.Controllers
{
    [Route("auth/")]
    [ApiController]
    public class AuthenticationController : ApiBaseController
    {
        public AuthenticationController(ISender sender)
            : base(sender)
        {
        }

        #region POST 

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
        {
            var command = new RegisterUserCommand(request.email,
                request.password,
                request.dateOfBirth,
                request.fullname,
                request.role);
            return await PrepareResponse(command);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var command = new LoginCommand(request.email,
                request.password);
            return await PrepareResponse(command);

        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            var command = new ForgetPasswordCommand(email);
            return await PrepareResponse(command);
        }
        #endregion


        #region GET
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAysnc(
           [FromQuery] string token,
           [FromQuery] string userId)
        {
            var command = new ConfirmEmailCommand(token, userId);
            return await PrepareResponse(command);
        }
        #endregion


        #region PUT
        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.email,
                request.newPassword,
                request.code);
            return await PrepareResponse(command);

        }
        [Authorize]
        [HttpPut("profilePicture")]

        public async Task<IActionResult> UpdateProfilePictureAsync(IFormFile picture)
        {
            var command = new SetProfilePictureCommand(picture);
            return await PrepareResponse(command);
        }
        #endregion


        #region Delete
        [Authorize]
        [HttpDelete("profilePicture")]
        public async Task<IActionResult> DeleteProfilePictureAsync()
        {
            var command = new DeleteProfilePictureCommand();
            return await PrepareResponse(command);
        }

        #endregion
    }
}
