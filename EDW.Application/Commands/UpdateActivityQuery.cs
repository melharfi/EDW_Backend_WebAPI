using MediatR;
using System;

namespace EDW.Application.Commands
{
    public class UpdateActivityQuery : IRequest
    {
        public UpdateActivityQuery(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public Guid Id { get; }
        public string Name { get; internal set; }
        public string Code { get; internal set; }
    }
}
