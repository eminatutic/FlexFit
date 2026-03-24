using MediatR;

namespace FlexFit.Application.Commands
{
    public class ProcessNoShowPenaltyCommand : IRequest<bool>
    {
        public int ReservationId { get; }

        public ProcessNoShowPenaltyCommand(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
