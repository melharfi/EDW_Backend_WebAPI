using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Queries
{
    public class GetAvailableUsersQueryHandler : IRequestHandler<GetAvailableUsersQuery, List<User>>
    {
        private readonly AppDbContext appDbContext;

        public GetAvailableUsersQueryHandler(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task<List<User>> Handle(GetAvailableUsersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(appDbContext.Users.Where(f => !string.IsNullOrEmpty(f.ActivityCode)).ToList());
        }
    }
}
