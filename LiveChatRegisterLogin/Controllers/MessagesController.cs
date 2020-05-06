using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.HubConfig;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Services;
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
        private IConnectionService _service;

        public MessagesController(DataContext context, IHubContext<MessagesHub> hub, IMessagesRepository repository, IConnectionService service)
        {
            _hub = hub;
            _repository = repository;
            _service = service;
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

            var messageChartModel = new MessageChartModel
            {
                ChatId = chat.Id,
                SenderName = user.Email,
                Content = message.Content,
                Date = messageDate
            };

            object[] param = { messageChartModel };
            IEnumerable<int> userIds = chat.ChatMemberships.Select(x => x.UserId);

            foreach(int userId in userIds)
            {
                string connectionId = _service.GetConnectionId(userId);
                if(connectionId != null && connectionId.Trim().Length != 0)
                {
                    await _hub.Clients.User(connectionId).SendAsync("transferchartdata", param).ConfigureAwait(true);
                }
            }
            

            return Ok(new { V = "received message" });
        }

        [HttpPost("getLastTwentyMessages")]
        public async Task<IActionResult> GetLastTwentyMessages(GetMessagesDTO getMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad Request");
            }

            var messages = await _repository.GetMessageCointainer(getMessage).ConfigureAwait(true);

            if(messages == null)
            {
                return NotFound("Not found any chats with requester id.");
            }

            return Ok(messages);
        }
    }
}