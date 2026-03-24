using FlexFit.Domain.Models;
using MediatR;

namespace FlexFit.Application.Queries
{
    public class GetFitnessObjectByIdQuery : IRequest<FitnessObject>
    {
        public int Id { get; set; }
        public GetFitnessObjectByIdQuery(int id)
        {
            Id = id;
        }
    }
}
