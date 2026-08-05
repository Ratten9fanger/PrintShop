using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;

namespace PrintShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<Guid> Login([FromBody] LoginRequest loginRequest)
        {
            var (token, error) = await userService.Login(loginRequest.Email, loginRequest.Password);

            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            Response.Cookies.Append("burmalda", token);

            return Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult<Guid> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.RepeatedPassword)
                return BadRequest("Passwords are not the same");

            var (user, error) = CreateUser(registerRequest);

            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            var userId = await userService.SignUp(user);

            return Ok(userId);
        }

    }
}
