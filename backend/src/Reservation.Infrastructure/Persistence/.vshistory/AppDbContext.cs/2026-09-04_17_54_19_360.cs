using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Common;
using ReservationAggregate = Reservation.Domain.Reservations.Reservation;
using System.Data;
namespace Reservation.Infrastructure.Persistence {

    public sealed class AppDbContext : DbContext, IUnitOfWork {
        private readonly IDomainEventDispatcher _dispatcher;
        public AppDbContext(DbContextOptions<AppDbContext>options,
                            IDomainEventDispatcher dispatcher):base(options) {
            _dispatcher = dispatcher;
        }
        public DbSet<ReservationAggregate> Reservations => Set<ReservationAggregate>();
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //scan other ModelBuilders in this "assemble"--namespace,thus,the "Configurations/ReservationConfiguration.cs"
            //will be apply in by "ApplyConfigurationsFromAssembly"
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        private List<IDomainEvent> CollectDomainEvents() {
            var aggregates = ChangeTracker.Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents.Count > 0)
                .Select(e => e.Entity)
                .ToList();
            var events = aggregates.SelectMany(x => x.DomainEvents).ToList();
            //clear to prevent re-dispatch
            aggregates.ForEach(x => x.ClearDomainEvents());
            return events;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) {
            //throw new NotImplementedException();
            var events = CollectDomainEvents();
            //ct--cancellation token is safe use here --able to rollback
            var affected = await base.SaveChangesAsync(ct);
            //if base.SaveChgnesAsync works.dispatch events,NOT CANCELLABLE!!
            if (events.Count > 0)
                await _dispatcher.DispatchAsync(events, CancellationToken.None);

            return affected;

        }

    }
}
