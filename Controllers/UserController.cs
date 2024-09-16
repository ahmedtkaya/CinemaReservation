using Microsoft.AspNetCore.Mvc;
using cinemaReservation.Models;
using cinemaReservation.Services;
using cinemaReservation.Helpers;

namespace cinemaReservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;
        public UserController(UserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Surname) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User Data is invalid");
            }
            await _userService.CreateAsync(user);
            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {

                return BadRequest("Email and password are required.");
            }

            var user = await _userService.GetByEmailAsync(loginRequest.Email);

            if (user == null || !PasswordHelper.VerifyPassword(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtHelper.GenerateJwtToken(user.Id);
            return Ok(new { Token = token });
        }
    }
}