using Reservation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Common;
using ReservationAggregate = Reservation.Domain.Reservations.Reservation;
using Reservation.Domain.Reservations;
using Microsoft.EntityFrameworkCore;
namespace Reservation.Infrastructure.Persistence {

    public sealed class EfReservationRepository(AppDbContext db) : IReservationRepository {
        public async Task AddAsync(ReservationAggregate reservation, CancellationToken ct=default) {
            await db.Reservations.AddAsync(reservation,ct); 
        }

        public async Task<ReservationAggregate?> FindAsync(ReservationId id, CancellationToken ct=default) {
            //return await db.Reservations.FindAsync(r => r.Id == id, ct)  <--錯誤示範!!你忘了Tables.了
            return await db.Reservations
                           .Include(r => r.Tables)
                           .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<IReadOnlyList<ReservationAggregate>> FindByDateAsync(DateOnly date, CancellationToken ct=default) {

            // 用區間比較而非 CAST(StartAt AS date) = @date：
            // 函式套在欄位上會使 IX_Reservations_StartAt 失效。

            var from = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.FromHours(8));
            var to = from.AddDays(1);
            return await db.Reservations
                           .Include(r => r.Tables)
                           .Where(r => r.Slot.StartAt >= from && r.Slot.StartAt < to)
                           .OrderBy(r => r.Slot.StartAt)
                           .ToListAsync(ct);

        }


    }
}
