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
    public class NotificationController : Controller
    {
        private INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getAllSent/{userId}")]
        public async Task<IActionResult> GetAllSentNotification(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            var notifications = await _repository.GetAllSentNotification(userId).ConfigureAwait(true);

            return Ok(notifications);
        }

        [HttpGet("getAllReceived/{userId}")]
        public async Task<IActionResult> GetAllReceivedNotification(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            var notifications = await _repository.GetAllReceivedNotification(userId).ConfigureAwait(true);

            return Ok(notifications);
        }

        [HttpPost("invitation")]
        public async Task<IActionResult> AddRelation(InvitatationDTO ivnitationDTO)
        {
            if (!ModelState.IsValid || ivnitationDTO == null)
            {
                return BadRequest("The body of request is not valid");
            }

            if(!int.TryParse(ivnitationDTO.UserId, out int userId) || !int.TryParse(ivnitationDTO.OtherId, out int newFriendId))
            {
                return BadRequest("Cannot parse uId or nId");
            }

            if (!await _repository.UserExists(userId).ConfigureAwait(true) || !await _repository.UserExists(newFriendId).ConfigureAwait(true))
            {
                return BadRequest("Cannot find any user.");
            }

            var isSuccess = await _repository.AddInvitation(userId, newFriendId).ConfigureAwait(true); 

            if (isSuccess)
            {
                return Ok(new string("Invitation has just sent"));
            }

            return BadRequest("Users have a relation.");
        }

        [HttpPost("deleteRelation")]
        public async Task<IActionResult> DeleteRelation(RelationDeletionDTO deleteRelation)
        {
            if (!ModelState.IsValid || deleteRelation == null)
            {
                return BadRequest("The body of request is not valid");
            }

            if (!int.TryParse(deleteRelation.SenderId, out int senderId))
            {
                return BadRequest("Cannot parse sender id.");
            }

            if (!int.TryParse(deleteRelation.ChatId, out int receiverId))
            {
                return BadRequest("Cannot parse chat id.");
            }

            bool isSuccess = await _repository.DeleteRelation(senderId, receiverId, deleteRelation.Reason).ConfigureAwait(true);

            if(isSuccess)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost("proccess")]
        public async Task<IActionResult> ProcessNotification(NotificationDTO notificationDTO)
        {
            if (!ModelState.IsValid || notificationDTO == null)
            {
                return BadRequest("The body of request is not valid");
            }

            if(!int.TryParse(notificationDTO.Id, out int notificationid))
            {
                return BadRequest("Cannot parse notification id.");
            }

            if (!int.TryParse(notificationDTO.UserId, out int userId))
            {
                return BadRequest("Cannot parse user id.");
            }

            bool isNotificationAccepted = notificationDTO.Action;

            try
            {
                await _repository.Process(notificationid, isNotificationAccepted, userId).ConfigureAwait(true);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}