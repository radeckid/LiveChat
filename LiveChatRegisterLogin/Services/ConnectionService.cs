using System.Collections.Generic;

namespace LiveChatRegisterLogin.Services
{
    public class ConnectionService : IConnectionService
    {
        public Dictionary<int, string> Connection { get; set; }

        public ConnectionService()
        {
            Connection = new Dictionary<int, string>();
        }

        public void AddConnection(int userId, string connectionId)
        {
            if(!Connection.ContainsKey(userId))
            {
                Connection.Add(userId, connectionId);
            }
        }

        public void DisposeConnection(int userId)
        {
            Connection.Remove(userId);
        }
    }
}
