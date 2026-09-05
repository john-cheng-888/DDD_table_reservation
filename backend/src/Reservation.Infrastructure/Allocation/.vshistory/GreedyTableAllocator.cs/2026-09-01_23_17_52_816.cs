using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Allocation {
    public sealed class GreedyTableAllocator(ITableCatalog catalog, IReservationRepository repository ) : ITableAllocator {
        private const int MaxCombinedTables = 3;//最多併三桌
        public async Task<IReadOnlyList<TableNo>> AllocateAsyn(
                TimeSlot slot,PartySize size,ReservationId? excluding=null,
                CancellationToken ct
            ) {
            var all = await catalog.GetAllAsync(ct);
            var sameDay = await repository.FindByDateAsync(DateOnly.FromDateTime(slot.StartAt.DateTime), ct);
            var occupied = sameDay
                .Where(r => excluding is null || r.Id != excluding.Value)
                .Where(r => r.Status is ReservationStatus.Confirmed or ReservationStatus.Seated)
                .Where(r => r.Slot.Overlaps(slot))
                .SelectMany(r => r.Tables)
                .ToHashSet();//減少重複項,因ValueObject可以推算HashCode.

        }
        
        
        public Task<IReadOnlyList<TableNo>> AllocateAsync(TimeSlot slot, PartySize size, ReservationId? excluding = null, CancellationToken ct = default) {
            throw new NotImplementedException();
        }
    }
}
