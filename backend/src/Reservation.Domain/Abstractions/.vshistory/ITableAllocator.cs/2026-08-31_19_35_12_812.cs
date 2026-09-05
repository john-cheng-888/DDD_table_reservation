using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface ITableAllocator {
        Task<TableNo?> AllocateAsync(TimeSlot slot, PartySize size, CancellationToken ct);
    }
}
