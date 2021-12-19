using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDW.Application.Models
{
    public class LoginResult
    {
        public string Token { get; set; }
        public bool Logged { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static LoginResult Fail() => new() { Logged = false };
        public static LoginResult Success(string token, string firstname, string lastname) => new() { Logged = true, Token = token, FirstName = firstname, LastName = lastname };
    }
}
