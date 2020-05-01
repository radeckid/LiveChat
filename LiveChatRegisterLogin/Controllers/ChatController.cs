using System;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveChatRegisterLogin.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        IChatRepository _repository;

        public ChatController(IChatRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getAll/{id}")]
        public async Task<IActionResult> GetAllChats(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            if(!int.TryParse(id, out int userId))
            {
                return BadRequest("Incorrect format id.");
            }

            var chats = _repository.GetChats(userId);

            if(chats == null || chats.Count == 0)
            {
                return Ok("You have not any chat");
            }

            return Ok(_repository.ConvertChats(chats, userId));
        }
    }
}
