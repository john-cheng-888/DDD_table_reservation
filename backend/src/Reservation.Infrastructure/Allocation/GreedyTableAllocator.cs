using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Allocation {
    public sealed class GreedyTableAllocator(ITableCatalog catalog, IReservationRepository repository ) : ITableAllocator {
        private const int MaxCombinedTables = 3;//最多併三桌
        public async Task<IReadOnlyList<TableNo>> AllocateAsync(
                TimeSlot slot,PartySize size,ReservationId? excluding=null,
                CancellationToken ct=default
            ) {
            var all = await catalog.GetAllAsync(ct);
            var sameDay = await repository.FindByDateAsync(DateOnly.FromDateTime(slot.StartAt.DateTime), ct);
            var occupied = sameDay
                .Where(r => excluding is null || r.Id != excluding.Value)
                .Where(r => r.Status is ReservationStatus.Confirmed or ReservationStatus.Seated)
                .Where(r => r.Slot.Overlaps(slot))
                .SelectMany(r => r.Tables)
                .ToHashSet();//減少重複項,因ValueObject可以推算HashCode.

            var free = all.Where(t => !occupied.Contains(t.No)).ToList();

            //rule1,能不併就不併--客戶觀感優先
            var single = free.Where(t => t.Capacity >= size.Value)
                           .OrderBy(t => t.Capacity)
                           .FirstOrDefault();
            if (single is not null) return [single.No];

            //rule2 :併桌:大桌優先,湊夠即止
            var combo = new List<TableNo>();
            var seats = 0;
            foreach (var t in free.OrderByDescending(t => t.Capacity)) {
                combo.Add(t.No);
                seats += t.Capacity;
                if (seats >= size.Value) return combo;
                if (combo.Count > MaxCombinedTables) break;
            }
            //好了,都查過了,放棄了
            return [];

        }
        
        
    }
}
