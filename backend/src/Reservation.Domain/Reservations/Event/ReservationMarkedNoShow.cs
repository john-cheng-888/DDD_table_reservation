using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public sealed record ReservationMarkedNoShow(
        ReservationId Id,
        DateTimeOffset OccurredAt
        ) : IDomainEvent;
}
