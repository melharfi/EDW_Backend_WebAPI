using EDW.Application.Exceptions;
using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Commands
{
    public class UpdateUserActivityQueryHandler : IRequestHandler<UpdateUserActivityQuery>
    {
        private readonly AppDbContext context;

        public UpdateUserActivityQueryHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Unit> Handle(UpdateUserActivityQuery request, CancellationToken cancellationToken)
        {
            User user = context.Users.FirstOrDefault(f => f.Username == request.Username);

            #region Check if ActivityCode value exist between available activities
            if (!context.Activities.Any(f => !string.IsNullOrEmpty(request.ActivityCode) && f.Code == request.ActivityCode))
                throw new ActivityNotFoundException();
            #endregion

            user.ActivityCode = request.ActivityCode;
            user.LastModifiedActivityTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
