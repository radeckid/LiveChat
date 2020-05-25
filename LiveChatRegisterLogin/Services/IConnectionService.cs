namespace LiveChatRegisterLogin.Services
{
    public interface IConnectionService
    {
        void AddConnection(string connectionId, int userId);
        void DisposeConnection(string connectionId);
        string GetConnectionId(int userId);
    }
}
