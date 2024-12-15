using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly WorkDbContext _context;

        public AuthController(IConfiguration configuration, WorkDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User model)
        {
            if (!_context.Users.Any(u => u.Username == model.Username && u.Password == model.Password))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            { 
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, model.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"] 
            }; 
          
            var token = tokenHandler.CreateToken(tokenDescriptor); 
          
            var tokenString = tokenHandler.WriteToken(token); 
           
            return Ok(new { Token = tokenString });
        }
    }
}