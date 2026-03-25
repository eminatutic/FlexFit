using FlexFit.Application.Commands;
using FlexFit.Domain.Models;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using MediatR;

namespace FlexFit.Application.Handlers
{
    public class CreateDailyCardCommandHandler : IRequestHandler<CreateDailyCardCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        public CreateDailyCardCommandHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<bool> Handle(CreateDailyCardCommand request, CancellationToken cancellationToken)
        {
            var existingCard = await _uow.MembershipCards.GetByCardNumberAsync(request.Dto.CardNumber);
            if (existingCard != null) return false;

            var card = new DailyCard
            {
                CardNumber = request.Dto.CardNumber,
                PurchaseDate = DateTime.UtcNow,
                IsActive = true,
                FitnessObjects = new List<FitnessObject>()
            };

            if (request.Dto.FitnessObjectIds != null)
            {
                foreach (var id in request.Dto.FitnessObjectIds)
                {
                    var fitnessObject = await _uow.FitnessObjects.GetByIdAsync(id);
                    if (fitnessObject != null)
                    {
                        card.FitnessObjects.Add(fitnessObject);
                    }
                }
            }

            await _uow.MembershipCards.AddAsync(card);
            
            try 
            {
                await _uow.MemberGraph.CreateCardAsync(card.CardNumber, "daily card");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateDailyCardHandler] WARNING: Neo4j sync failed: {ex.Message}");
            }

            await _uow.SaveAsync();
            return true;
        }
    }
}