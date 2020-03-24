using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveChatRegisterLogin.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly DataContext _context;

        public MessagesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("message")]
        public async Task<IActionResult> Test([FromBody] MessageDTO message)
        {
            if (!ModelState.IsValid || message == null)
            {
                return BadRequest("Bad Request");
            }

            //musimy sprawdzać czy użytkownik z danym id jest w bazie wgl

            if (await _context.Users.AnyAsync(x => x.Id == int.Parse(message.SenderId)).ConfigureAwait(true) ||
                await _context.Users.AnyAsync(x => x.Id == int.Parse(message.ReciverId)).ConfigureAwait(true))
            {
                return BadRequest("One of the users doesn't exist");
            }

            var addMessage = new Message()
            {
                SenderIdFK = int.Parse(message.SenderId),
                ReciverIdFK = int.Parse(message.ReciverId),
                Time = message.Time,
                Content = message.Content,
            };

            await _context.Messages.AddAsync(addMessage).ConfigureAwait(true);
            await _context.SaveChangesAsync().ConfigureAwait(true);

            return Ok(message);
        }


    }
}