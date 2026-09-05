using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public sealed record ReservationTablesReassigned(
        ReservationId Id,
        IReadOnlyList<string> Before,
        IReadOnlyList<string> After,
        string Reason,
        DateTimeOffset OccurredAt
        ) : IDomainEvent;
}
