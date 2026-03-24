using FlexFit.Application.Commands;
using FlexFit.Domain.Models;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using MediatR;

namespace FlexFit.Application.Handlers
{
    public class ProcessNoShowPenaltyCommandHandler : IRequestHandler<ProcessNoShowPenaltyCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ProcessNoShowPenaltyCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(ProcessNoShowPenaltyCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _uow.Reservations.GetByIdAsync(request.ReservationId);
            if (reservation == null) return false;

            if (reservation.Status == ReservationStatus.NoShow)
            {
                var penaltyPoint = new PenaltyPoint
                {
                    MemberId = reservation.MemberId,
                    Description = $"Automatski kazneni poen zbog nedolaska na termin (Rezervacija ID: {reservation.Id})",
                    Date = DateTime.UtcNow
                };

                await _uow.PenaltyPoints.AddAsync(penaltyPoint);
                await _uow.SaveAsync();
                return true;
            }

            return false;
        }
    }
}
