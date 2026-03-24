using FlexFit.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace FlexFit.Application.Queries
{
    public class GetAllMembershipCardsQuery : IRequest<IEnumerable<MembershipCard>>
    {
        public string? Type { get; set; }
    }
}
