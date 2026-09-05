using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface ITableAllocator {
        Task<IReadOnlyList<TableNo>> AllocateAsync(
            TimeSlot slot, PartySize size, 
            //換桌時要把目前這桌的物件排除
            ReservationId? excluding=null,
            CancellationToken ct=default);
    }
}
