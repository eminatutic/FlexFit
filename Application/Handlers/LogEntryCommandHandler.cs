using FlexFit.Application.Commands;
using FlexFit.Domain.Models;
using FlexFit.Domain.MongoModels.Models;
using FlexFit.Domain.MongoModels.Repositories;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using MediatR;

using FlexFit.Domain.Interfaces.Repositories;

namespace FlexFit.Application.Handlers
{
    public class LogEntryCommandHandler : IRequestHandler<LogEntryCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly EntryLogRepository _mongoRepo;
        private readonly IMemberGraphRepository _graphRepo;

        public LogEntryCommandHandler(IUnitOfWork uow, EntryLogRepository mongoRepo, IMemberGraphRepository graphRepo)
        {
            _uow = uow;
            _mongoRepo = mongoRepo;
            _graphRepo = graphRepo;
        }

        public async Task<bool> Handle(LogEntryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 1. Log to MongoDB
            var log = new EntryLog
            {
                MemberId = dto.MemberId,
                EmployeeId = dto.EmployeeId,
                Time = DateTime.UtcNow,
                CardStatus = dto.CardStatus,
                CardType = dto.CardType,
                Incident = (dto.CardStatus != "Active" && dto.CardStatus != "Aktivna") 
                    ? "Unauthorized entry attempt" 
                    : null
            };

            await _mongoRepo.AddAsync(log);

            // Sync visit to Neo4j (Simplified: IDs only)
            if (dto.MemberId > 0)
            {
                await _graphRepo.RecordVisitAsync(dto.MemberId.ToString(), dto.FitnessObjectId);
            }

            // 2. Automated Penalty for Daily/Subscription card violations
            bool isExpired = dto.CardStatus != "Active" && dto.CardStatus != "Aktivna";
            bool isDailyOrSub = dto.CardType == "Daily" || dto.CardType == "Dnevna" || 
                                dto.CardType == "Subscription" || dto.CardType == "Pretplatna";

            if (isExpired && isDailyOrSub && dto.MemberId > 0)
            {
                // Provera da li je kazna veÄ‡ izdata u poslednjih 12h
                bool hasRecentPenalty = await _uow.PenaltyCards.HasRecentPenaltyAsync(dto.MemberId, 12);

                if (!hasRecentPenalty)
                {
                    // Automatska kazna: Cena dnevne karte (npr. 500) + 500 = 1000
                    var penalty = new PenaltyCard
                    {
                        MemberId = dto.MemberId,
                        FitnessObjectId = dto.FitnessObjectId,
                        Date = DateTime.UtcNow,
                        Price = 1000, // 500 (daily) + 500 (extra)
                        Reason = $"Automatski izdata kazna zbog nevaÅ¾eÄ‡e kartice ({dto.CardNumber}). Tip: {dto.CardType}, Status: {dto.CardStatus}"
                    };

                    await _uow.PenaltyCards.AddAsync(penalty);
                    await _uow.SaveAsync();
                }
            }

            // 3. Mark active/upcoming reservation as 'Used'
            if (dto.MemberId > 0)
            {
                var now = DateTime.UtcNow;
                var reservations = await _uow.Reservations.FindAsync(r => 
                    r.MemberId == dto.MemberId && 
                    r.Status == ReservationStatus.Reserved &&
                    r.StartTime <= now.AddMinutes(30) && // Started in the last 30 minutes or starting in the next 30
                    r.EndTime >= now.AddMinutes(-30));

                foreach (var res in reservations)
                {
                    res.Status = ReservationStatus.Used;
                    await _uow.Reservations.UpdateAsync(res);
                }
                
                if (reservations.Any())
                {
                    await _uow.SaveAsync();
                }
            }

            return true;
        }
    }
}
