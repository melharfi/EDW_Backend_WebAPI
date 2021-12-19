using EDW.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace EDW.Application.Queries
{
    public class GetAllActivitiesQuery : IRequest<List<Activity>>
    {

    }
}
