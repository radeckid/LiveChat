using System;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.HubConfig;
using LiveChatRegisterLogin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveChatRegisterLogin.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly DataContext _context;
        private IHubContext<MessagesHub> _hub;

        public MessagesController(DataContext context, IHubContext<MessagesHub> hub)
        {
            _context = context;
            _hub = hub;
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

            User sender;
            User receiver;

            //musimy sprawdzać czy użytkownik z danym id jest w bazie wgl
            if(int.TryParse(message.SenderId, out int senderId) && int.TryParse(message.ReceiverId, out int receiverId))
            {
                sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId).ConfigureAwait(true);
                receiver = await _context.Users.FirstOrDefaultAsync(x => x.Id == receiverId).ConfigureAwait(true);
            }
            else
            {
                return BadRequest("SenderId or receiverId has a wrong format.");
            }

            if(sender == null || receiver == null)
            {
                return BadRequest("One of the users doesn't exist");
            }

            var addMessage = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Date = messageDate,
                Content = message.Content,
            };

            await _context.Messages.AddAsync(addMessage).ConfigureAwait(true);
            await _context.SaveChangesAsync().ConfigureAwait(true);


            object[] param = { new MessageChartModel { ReceiverId = receiverId } };
            _ = _hub.Clients.All.SendCoreAsync("transfermesasges", param).ConfigureAwait(true);

            return Ok(message);
        }


    }
}