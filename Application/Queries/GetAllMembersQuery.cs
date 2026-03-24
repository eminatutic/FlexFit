using FlexFit.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace FlexFit.Application.Queries
{
    public class GetAllMembersQuery : IRequest<IEnumerable<Member>>
    {
    }
}
