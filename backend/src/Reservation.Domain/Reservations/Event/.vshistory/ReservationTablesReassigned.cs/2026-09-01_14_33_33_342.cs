using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public sealed record ReservationTablesReassigned(
        ReservationId,
        IReadOnlyList<string> tableNosBefore,
        IReadOnlyList<string> tableNosReassgied,
        string reason,
        DateTimeOffset OccurredAt
        ) : IDomainEvent;
}
