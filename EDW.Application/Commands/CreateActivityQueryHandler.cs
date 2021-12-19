using EDW.Application.Exceptions;
using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Commands
{
    public class CreateActivityQueryHandler : IRequestHandler<CreateActivityQuery, Guid>
    {
        private readonly AppDbContext appDbContext;

        public CreateActivityQueryHandler(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(CreateActivityQuery request, CancellationToken cancellationToken)
        {
            if (appDbContext.Activities.Any(f => f.Name == request.Name))
                throw new ActivityNameDuplicationException();

            if (appDbContext.Activities.Any(f => f.Code == request.Code))
                throw new ActivityCodeDuplicationException();

            Activity activity = new()
            {
                Name = request.Name,
                Code = request.Code
            };
            await appDbContext.Activities.AddAsync(activity, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return activity.Id;
        }
    }
}
