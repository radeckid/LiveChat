using System;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.Data;
using LiveChatRegisterLogin.DTO;
using LiveChatRegisterLogin.Models;
using LiveChatRegisterLogin.Types;
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

            var notifications = _repository.GetAllSentNotification(userId);

            return Ok(notifications);
        }

        [HttpGet("getAllReceived/{userId}")]
        public async Task<IActionResult> GetAllReceivedNotification(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The body of request is not valid");
            }

            var notifications = _repository.GetAllReceivedNotification(userId);

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

            NotificationType type = NotificationType.Invitation;
            int chatId = 0;

            if(!ivnitationDTO.ChatId.IsNullOrEmpty() && int.TryParse(ivnitationDTO.ChatId, out chatId) && !chatId.Equals(0))
            {
                type = NotificationType.GroupInvitation;
            }

            var isSuccess = await _repository.AddInvitation( new InvitationContainer 
            { 
                RequesterId = userId,
                ReceiverId = newFriendId,
                ChatId = chatId,
                Type = type
            }).ConfigureAwait(true); 

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

            if (!int.TryParse(deleteRelation.ChatId, out int chatId))
            {
                return BadRequest("Cannot parse chat id.");
            }

            int memberId = default;
            if (!deleteRelation.MemberId.IsNullOrEmpty() && !int.TryParse(deleteRelation.MemberId, out memberId))
            {
                return BadRequest("Cannot parse member id.");
            }

            bool isSuccess = await _repository.DeleteMembership(new DeletionMemberContainer
            { 
                SenderId = senderId,
                ChatId = chatId,
                Reason = deleteRelation.Reason,
                MemberId = memberId
            }).ConfigureAwait(true);

            if(isSuccess)
            {
                return Ok(true);
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