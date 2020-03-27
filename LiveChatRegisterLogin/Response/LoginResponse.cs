using LiveChatRegisterLogin.Models;
using System.Text.Json.Serialization;

namespace LiveChatRegisterLogin.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public User User { get; set; }
    }
}
