using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;

namespace PrintShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<Guid> Login([FromBody] LoginRequest)
        {
            var (token, error) = await userService.Login(LoginRequest.Email, LoginRequest.Password);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            Response.Cookies.Append("burmalda", token);

            return Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult<Guid> Register([FromBody] RegisterRequest)
        {
            //Создаем пользователя в коде и можем возвратить ошибку
            //после прохождения валидации вызываем сервис

            if (RegisterRequest.Password != RegisterRequest.RepeatedPassword)
                return BadRequest("Passwords are not the same");

            var isUserExists = await userService.CheckUserExistence(RegisterRequest.Email);

            if (isUserExists)
                return BadRequest("User with this name is already exists");

            var (user, error) = TodoApi.Core.Models.User.Create(Guid.NewGuid(), userRequest.Name, userRequest.Password);

            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            var userId = await userService.SignUp(user);

            return Ok(userId);
        }

    }
}
