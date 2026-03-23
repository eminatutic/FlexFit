using FlexFit.Application.Commands;
using FlexFit.Models;
using FlexFit.UnitOfWorkLayer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlexFit.Application.Services
{
    public class ReservationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);

        public ReservationBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessNoShowsAsync();
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessNoShowsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var now = DateTime.UtcNow;
            
            // Pronađi sve rezervacije koje su istekle a nisu iskorišćene
            var expiredReservations = await uow.Reservations.FindAsync(r => 
                r.Status == ReservationStatus.Reserved && 
                r.EndTime < now);

            foreach (var reservation in expiredReservations)
            {
                try
                {
                    reservation.Status = ReservationStatus.NoShow;
                    await uow.Reservations.UpdateAsync(reservation);
                    await uow.SaveAsync();

                    // Pokreni komandu za dodeljivanje kaznenih poena
                    await mediator.Send(new ProcessNoShowPenaltyCommand(reservation.Id));
                }
                catch (Exception ex)
                {
                    // Možemo dodati logovanje ovde ako je potrebno
                    Console.WriteLine($"Greška pri obradi no-show rezervacije {reservation.Id}: {ex.Message}");
                }
            }
        }
    }
}
