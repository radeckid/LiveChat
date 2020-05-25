using LiveChatRegisterLogin.Containers;
using LiveChatRegisterLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveChatRegisterLogin.Services
{
    public abstract class ConnectionService : IConnectionService
    {
        private IList<ConnectionContainer> _connections;

        public ConnectionService()
        {
            _connections = new List<ConnectionContainer>();
        }

        public void AddConnection(string connectionId, int userId)
        {
            _connections.Add(new ConnectionContainer
            {
                ConnectionId = connectionId,
                UserId = userId
            });
        }

        public void DisposeConnection(string connectionId)
        {
            var connection = _connections.FirstOrDefault(x => x.ConnectionId.Equals(connectionId, StringComparison.CurrentCulture));

            if (connection != null)
            {
                _connections.Remove(connection);
            }
        }

        public string GetConnectionId(int userId)
        {
            return _connections.FirstOrDefault(x => x.UserId.Equals(userId))?.ConnectionId;
        }
    }
}
