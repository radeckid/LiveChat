using LiveChatRegisterLogin.Data;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace LiveChatRegisterLogin.HubConfig
{
    [Authorize]
    public class GateHub : Hub
    {
        private DataContext _context;

        public GateHub(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> GetConnectionId(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(true);
            var requesterMail = Context.User.Identity.Name;
            var ipAddress = Context.Features.Get<IHttpConnectionFeature>()?.ConnectionId;
            bool isSuccessLogged;

            if (requesterMail.Equals(requesterMail, System.StringComparison.CurrentCulture))
            {
                isSuccessLogged = true;
            }
            else
            {
                isSuccessLogged = false;
            }

            await _context.UsersTrace.AddAsync(new Models.UserTrace
            {
                Who = requesterMail,
                UserIdProvided = userId,
                When = DateTime.Now,
                Result = isSuccessLogged,
                IpAddress = ipAddress
            });

            await _context.SaveChangesAsync().ConfigureAwait(true);

            return isSuccessLogged;
        }
    }
}
