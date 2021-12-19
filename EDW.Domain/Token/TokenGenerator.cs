using EDW.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EDW.Domain.Token
{
    public class TokenGenerator : ITokenGenerator
    {
        public string Generate(User user)
        {
            // TODO should i secure this string ?
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0FFA06DD318FB46565DB729A60A3154D"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("Username", user.Username),
                new Claim("FullName", user.FirstName + " " + user.LastName)
            };

            var token = new JwtSecurityToken("EDWAPI", "EDWAPI", expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials, claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
