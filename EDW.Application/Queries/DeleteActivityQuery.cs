using MediatR;
using System;

namespace EDW.Application.Queries
{
    public class DeleteActivityQuery : IRequest
    {
        public DeleteActivityQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; internal set; }
    }
}
