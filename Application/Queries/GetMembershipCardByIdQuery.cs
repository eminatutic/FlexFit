using FlexFit.Domain.Models;
using MediatR;

namespace FlexFit.Application.Queries
{
    public class GetMembershipCardByIdQuery : IRequest<MembershipCard>
    {
        public int Id { get; set; }
        public GetMembershipCardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
