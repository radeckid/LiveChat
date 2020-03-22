using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LiveChatRegisterLogin.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public UsersController(IConfiguration config, DataContext context)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            User result = await _context.Users
                                .Where(p => p.Email == userDTO.Email && p.Password == userDTO.Password)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(true);

            if (result == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                new Claim(ClaimTypes.Name, result.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserDTO userDTO)
        {
            if (!ModelState.IsValid || userDTO == null)
            {
                return BadRequest("The body of request is not valid.");
            }
            bool result = await _context.Users
                                .AnyAsync(p => p.Email == userDTO.Email)
                                .ConfigureAwait(true);

            if (!result)
            {
                await _context.Users.AddAsync(new User() { Email = userDTO.Email, Password = userDTO.Password });
                await _context.SaveChangesAsync().ConfigureAwait(true);

                return Ok();
            }
            else
            {
                return BadRequest("User already exist");
            }
        }
    }
}