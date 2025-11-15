using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly UserStore store;
        private readonly TokenService token;

        public AuthController(UserStore _store,TokenService _token)
        {
            store = _store;
            token = _token;
            
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (store.Users.Any(u => u.Username == dto.Username))
                return BadRequest("username already existed");

            store.Users.Add(new User
            {
                Username=dto.Username,
                Password=dto.Password,
                Role=dto.Role ?? "User"
            });

            return Ok("Registered");
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = store.Users.FirstOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var Token = token.CreateToken(user);
            return Ok(new { token = Token });
        }
    }
    public record RegisterDto(string Username,string Password,string? Role);
    public record LoginDto(string Username,string Password);
}
