using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.HubConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiveChatRegisterLogin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private IHubContext<NotificationHub> _hub;
        private INotificationRepository _repository;

        public NotificationController(IHubContext<NotificationHub> hub, INotificationRepository repository)
        {
            _hub = hub;
            _repository = repository;
        }

        [HttpPost("invitation")]
        public async Task<IActionResult> AddRelation(int userId, int newFriendId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            if (!await _repository.UserExists(userId).ConfigureAwait(true) || !await _repository.UserExists(newFriendId).ConfigureAwait(true))
            {
                return BadRequest("Cannot find any user.");
            }

            var notification = await _repository.AddInvitation(userId, newFriendId).ConfigureAwait(true);

            if (notification == null)
            {
                return BadRequest("Users have a relation.");
            }

            object[] param = { new NotificationChartModel
            {
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId,
                Type = notification.Type,
                Desc = notification.Desc,
                ExtraData = notification.ExtraData,
                Date = notification.Date
            } };

            _ = _hub.Clients.All.SendCoreAsync("transfernotifications", param).ConfigureAwait(true);

            return Ok(new string("Invitation has just sent"));
        }

        [HttpPost("notification")]
        public async Task<IActionResult> ProcessNotification(NotificationDTO notificationDTO)
        {
            if (!ModelState.IsValid || notificationDTO == null)
            {
                return BadRequest("The body of request is not valid");
            }

            await _repository.Process(notificationDTO).ConfigureAwait(true);

            return Ok();
        }
    }
}