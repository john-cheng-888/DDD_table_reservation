using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Common;
using ReservationAggregate = Reservation.Domain.Reservations.Reservation;
namespace Reservation.Infrastructure.Persistence {
    public sealed class AppDbContext : DbContext, IUnitOfWork {
        public Task<int> SaveChangesAsyn(CancellationToken ct = default) {
            //throw new NotImplementedException();
        }
    }
}
