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
        public Task<int> SaveChangesAsyn(CancellationToken ct = default) {
            //throw new NotImplementedException();
        }
    }
}
