using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public sealed record ReservationTablesReassigned(
        ReservationId id,
        IReadOnlyList<string> Before,
        IReadOnlyList<string> After,
        string reason,
        DateTimeOffset OccurredAt
        ) : IDomainEvent;
}
