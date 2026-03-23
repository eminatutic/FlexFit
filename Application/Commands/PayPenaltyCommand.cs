using MediatR;

namespace FlexFit.Application.Commands
{
    public record PayPenaltyCommand(int PenaltyId) : IRequest<bool>;
}
