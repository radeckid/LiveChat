using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveChatRegisterLogin.Services
{
    public class ConnectionService : IConnectionService, IUserIdProvider
    {
        public IList<ConnectionContainer> Connections { get; set; }

        public ConnectionService()
        {
            Connections = new List<ConnectionContainer>();
        }

        public bool AddConnection(string connectionId, int userId)
        {
            if(Connections.Any(x => x.UserId == userId))
            {
                return false;
            }

            Connections.Add(new ConnectionContainer
            {
                UserId = userId,
                ConnectionId = connectionId
            });

            return true;
        }

        public void DisposeConnection(string connectionId)
        {
            var connection = Connections.FirstOrDefault(x => x.ConnectionId.Equals(connectionId, StringComparison.InvariantCulture));
            Connections.Remove(connection);
        }

        public bool HasUserId(int userId)
        {
            return Connections.Any(x => x.UserId == userId);
        }

        public bool HasConnection(string connectionId)
        {
            return Connections.Any(x => x.ConnectionId.Equals(connectionId, StringComparison.InvariantCulture));
        }

        public string GetConnectionId(int userId)
        {
            return Connections.FirstOrDefault(x => x.UserId == userId)?.ConnectionId ?? string.Empty;
        }

        public string GetUserId(HubConnectionContext connection)
        {
            string b = connection.User.Identity.Name;

            return b;
        }
    }
}
