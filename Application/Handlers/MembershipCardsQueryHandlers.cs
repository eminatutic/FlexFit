using FlexFit.Application.Queries;
using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlexFit.Application.Handlers
{
    public class GetAllMembershipCardsQueryHandler : IRequestHandler<GetAllMembershipCardsQuery, IEnumerable<MembershipCard>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllMembershipCardsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<MembershipCard>> Handle(GetAllMembershipCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = await _uow.MembershipCards.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                if (request.Type.Equals("daily", System.StringComparison.OrdinalIgnoreCase))
                {
                    return cards.Where(c => c is DailyCard);
                }
                else if (request.Type.Equals("subscription", System.StringComparison.OrdinalIgnoreCase))
                {
                    return cards.Where(c => c is SubscriptionCard);
                }
            }

            return cards;
        }
    }

    public class GetMembershipCardByIdQueryHandler : IRequestHandler<GetMembershipCardByIdQuery, MembershipCard>
    {
        private readonly IUnitOfWork _uow;

        public GetMembershipCardByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<MembershipCard> Handle(GetMembershipCardByIdQuery request, CancellationToken cancellationToken)
        {
            return await _uow.MembershipCards.GetByIdAsync(request.Id);
        }
    }
}
