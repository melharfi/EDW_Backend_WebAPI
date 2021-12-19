using EDW.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDW.Application.Queries
{
    public class GetAvailableUsersQuery : IRequest<List<User>>
    {
    }
}
