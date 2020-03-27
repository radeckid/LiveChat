using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LiveChatRegisterLogin.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;

        private CultureInfo cultureInfo = new CultureInfo("pl-PL", false);

        public UsersController(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            var result = await _repository.Login(userDTO.Email.ToLower(cultureInfo), userDTO.Password).ConfigureAwait(true);

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

            var response = new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                User = result
            };

            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDTO userDTO)
        {
            if (!ModelState.IsValid || userDTO == null)
            {
                return BadRequest("The body of request is not valid");
            }

            string email = userDTO.Email.ToLower(cultureInfo);

            if (await _repository.UserExists(email).ConfigureAwait(true))
            {
                return BadRequest("User already exists");
            }

            var usertoBeCreated = new User
            {
                Email = email,
                ReceivedMessages = new List<Message>(),
                SentMessages = new List<Message>(),
                FirstFriends = new List<Relation>(),
                SecondFriends = new List<Relation>()
            };

            var newUser = await _repository.Register(usertoBeCreated, userDTO.Password).ConfigureAwait(true);

            //201 because its 'created' status
            return StatusCode(201);
        }

        [HttpPost("friends")]
        public async Task<IActionResult> AddRelation(int userId, int newFriendId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            if (! await _repository.UserExists(userId).ConfigureAwait(true) || ! await _repository.UserExists(newFriendId).ConfigureAwait(true))
            {
                return BadRequest("Cannot find any user.");
            }

            if(_repository.IsRelation(userId, newFriendId))
            {
                return BadRequest("Users have a relation.");
            }
        }

        [HttpGet("friends/{userId}")]
        public async Task<IActionResult> GetAllFriends(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            var users = await _repository.GetAllFriend(userId);

            if(users == null)
            {
                return BadRequest("Cannot find user with provided id.");
            }

            return Ok(users);
        }
    }
}