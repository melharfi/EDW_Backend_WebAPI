using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDW.Application.Commands
{
    public class CreateActivityQuery : IRequest<Guid>
    {
        public CreateActivityQuery(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; internal set; }
        public string Code { get; internal set; }
    }
}
