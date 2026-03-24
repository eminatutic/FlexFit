using FlexFit.Infrastructure.Data;
using FlexFit.Infrastructure.Repositories;
using FlexFit.Infrastructure.Repositories.Interfaces;
using FlexFit.Domain.Interfaces.Repositories;
using FlexFit.Repositoires;
namespace FlexFit.Infrastructure.UnitOfWorkLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMemberGraphRepository _graphRepo;

        public UnitOfWork(AppDbContext context, IMemberGraphRepository graphRepo)
        {
            _context = context;
            _graphRepo = graphRepo;
            Members = new MemberRepository(_context);
            Employees = new EmployeeRepository(_context);
            FitnessObjects = new FitnessObjectRepository(_context);
            Resources = new ResourceRepository(_context);
            Reservations = new ReservationRepository(_context, _graphRepo);
            PenaltyCards = new PenaltyCardRepository(_context);
            PenaltyPoints = new PenaltyPointRepository(_context);
            MembershipCards = new MembershipCardRepository(_context);
        }

        public IMemberRepository Members { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IFitnessObjectRepository FitnessObjects { get; private set; }
        public IResourceRepository Resources { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public IPenaltyCardRepository PenaltyCards { get; private set; }
        public IPenaltyPointRepository PenaltyPoints { get; private set; }
        public IMembershipCardRepository MembershipCards { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}