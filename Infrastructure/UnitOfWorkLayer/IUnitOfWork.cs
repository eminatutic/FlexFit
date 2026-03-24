
using FlexFit.Infrastructure.Repositories.Interfaces;

namespace FlexFit.Infrastructure.UnitOfWorkLayer
{
    public interface IUnitOfWork
    {
        IMemberRepository Members { get; }
        IEmployeeRepository Employees { get; }
        IFitnessObjectRepository FitnessObjects { get; }
        IResourceRepository Resources { get; }
        IReservationRepository Reservations { get; }
        IPenaltyCardRepository PenaltyCards { get; }
        IPenaltyPointRepository PenaltyPoints { get; }
        IMembershipCardRepository MembershipCards { get; }

        Task SaveAsync();
    }
}