using MediatR;
using System;

namespace EDW.Application.Commands
{
    public class UpdateUserActivityQuery : IRequest
    {
        public UpdateUserActivityQuery(string username, string activityCode)
        {
            Username = username;
            ActivityCode = activityCode;
        }

        public string Username { get; }
        public string ActivityCode { get; internal set; }
    }
}
