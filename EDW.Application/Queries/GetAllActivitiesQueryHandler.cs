using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Queries
{
    public class GetAllActivitiesQueryHandler : IRequestHandler<GetAllActivitiesQuery, List<Activity>>
    {
        private readonly AppDbContext context;
        public GetAllActivitiesQueryHandler(AppDbContext context)
        {
            this.context = context;
        }
        public Task<List<Activity>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(context.Activities.ToList());
        }
    }
}
