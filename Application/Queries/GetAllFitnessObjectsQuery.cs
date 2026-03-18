using FlexFit.Models;
using MediatR;
using System.Collections.Generic;

namespace FlexFit.Application.Queries
{
    public class GetAllFitnessObjectsQuery : IRequest<IEnumerable<FitnessObject>>
    {
    }
}
