using EDW.Application.Models;
using MediatR;

namespace EDW.Application.Queries
{
    public class LoginQuery : IRequest<LoginResult>
    {
        public LoginQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }
}
