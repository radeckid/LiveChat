using System;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.HubConfig;
using LiveChatRegisterLogin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiveChatRegisterLogin.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IHubContext<MessagesHub> _hub;
        private IMessagesRepository _repository;

        public MessagesController(DataContext context, IHubContext<MessagesHub> hub, IMessagesRepository repository)
        {
            _hub = hub;
            _repository = repository;
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostMessage([FromBody] MessageDTO message)
        {
            if (!ModelState.IsValid || message == null)
            {
                return BadRequest("Bad Request");
            }
            
            if(!DateTime.TryParse(message.Date, out DateTime messageDate))
            {
                return BadRequest("Provided datetime is not valid.");
            }

            Chat chat;
            User user;

            if (int.TryParse(message.SenderId, out int senderId) && int.TryParse(message.ChatId, out int chatid))
            {
                (chat, user) = await _repository.GetMessageCointainer(senderId, chatid).ConfigureAwait(true);
            }
            else
            {
                return BadRequest("SenderId or receiverId has a wrong format.");
            }

            if(chat == null || user == null)
            {
                return BadRequest("Cannot find chat or user.");
            }

            await _repository.SendMessage(new Message
            {
                Sender = user,
                SenderName = user.Email,
                Chat = chat,
                Date = messageDate,
                Content = message.Content
            }).ConfigureAwait(true);

            object[] param = { new MessageChartModel { ChatId = chat.Id } };
            await _hub.Clients.All.SendAsync("transferchartdata", param).ConfigureAwait(true);

            return Ok(new { V = "received message" });
        }

        [HttpGet("getAll/{userId}/{chatId}")]
        public async Task<IActionResult> GetAll(string userId, string chatId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            Chat chat;
            User user;

            if (int.TryParse(userId, out int senderId) && int.TryParse(chatId, out int chatid))
            {
                (chat, user) = await _repository.GetMessageCointainer(senderId, chatid).ConfigureAwait(true);
            }
            else
            {
                return BadRequest("SenderId or receiverId has a wrong format.");
            }

            if (chat == null || user == null)
            {
                return BadRequest("Cannot find chat or user.");
            }

            return Ok(chat.Messages);
        }
    }
}