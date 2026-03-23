using FlexFit.Application.Commands;
using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using MediatR;

namespace FlexFit.Application.Handlers
{
    public class ExtendMembershipCommandHandler : IRequestHandler<ExtendMembershipCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ExtendMembershipCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(ExtendMembershipCommand request, CancellationToken cancellationToken)
        {
            var card = await _uow.MembershipCards.GetByCardNumberAsync(request.CardNumber);

            if (card == null || !(card is SubscriptionCard subCard))
            {
                return false;
            }

            // Set the start date to current date and end date to 30 days from now
            subCard.ValidFrom = DateTime.UtcNow;
            subCard.ValidTo = DateTime.UtcNow.AddDays(30);
            subCard.IsActive = true;

            await _uow.MembershipCards.UpdateAsync(subCard);
            await _uow.SaveAsync();

            return true;
        }
    }
}
