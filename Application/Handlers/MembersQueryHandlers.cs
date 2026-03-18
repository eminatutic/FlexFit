using FlexFit.Application.Queries;
using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlexFit.Application.Handlers
{
    public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<Member>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllMembersQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Member>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            return await _uow.Members.GetAllAsync();
        }
    }
}
