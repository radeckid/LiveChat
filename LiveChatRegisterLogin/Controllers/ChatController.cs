using System;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
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

            return Ok(_repository.ConvertChats(chats, userId));
        }

        [HttpPost("createGroupChat")]
        public async Task<IActionResult> CreateGroupChat(GroupChatDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            if (!int.TryParse(dto.OwnerId, out int ownerIdFormatted))
            {
                return BadRequest("Incorrect format id.");
            }

            var chat = await _repository.CreateChat(Types.ChatType.GroupChat, ownerIdFormatted, dto.Name).ConfigureAwait(true);

            if(chat == null)
            {
                return BadRequest("Bad result. Sory");
            }

            return Ok(new ChatDTO
            {
                Id = chat.Id,
                Name = chat.Name,
                OwnerId = chat.OwnerId,
                Type = chat.Type
            });
        }

        [HttpGet("getChatMembers/{id}")]
        public async Task<IActionResult> GetChatMembers(int id)
        {
            var list = await _repository.GetAllMembershipFromChat(id).ConfigureAwait(true);

            return Ok(list);
        }
    }
}
