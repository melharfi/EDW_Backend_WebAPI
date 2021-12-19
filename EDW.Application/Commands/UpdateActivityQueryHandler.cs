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
    public class UpdateActivityQueryHandler : IRequestHandler<UpdateActivityQuery>
    {
        private readonly AppDbContext context;

        public UpdateActivityQueryHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Unit> Handle(UpdateActivityQuery request, CancellationToken cancellationToken)
        {
            #region checks
            Activity original = context.Activities.FirstOrDefault(f => f.Id == request.Id);
            if (original == null)
                throw new ActivityNotFoundException();
            else
            {
                if (context.Activities.Any(f => f.Id != request.Id && f.Name == request.Name))
                    throw new ActivityNameDuplicationException();

                if (context.Activities.Any(f => f.Id != request.Id && f.Code == request.Code))
                    throw new ActivityCodeDuplicationException();
            }
            #endregion

            Activity activity = new()
            {
                Id = request.Id,
                Name = request.Name,
                Code = request.Code
            };

            context.Update(activity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
