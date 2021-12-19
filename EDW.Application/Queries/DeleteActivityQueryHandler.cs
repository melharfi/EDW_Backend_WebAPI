using EDW.Application.Exceptions;
using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Queries
{
    public class DeleteActivityQueryHandler : IRequestHandler<DeleteActivityQuery>
    {
        private readonly AppDbContext context;
        public DeleteActivityQueryHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Unit> Handle(DeleteActivityQuery request, CancellationToken cancellationToken)
        {
            #region checks
            Activity original = context.Activities.FirstOrDefault(f => f.Id == request.Id);
            if (original == null)
                throw new ActivityNotFoundException();
            #endregion

            context.Activities.Remove(original);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
