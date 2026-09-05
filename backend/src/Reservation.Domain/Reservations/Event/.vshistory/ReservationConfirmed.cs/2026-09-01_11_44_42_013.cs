using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public sealed record ReservationConfirmed(
         ReservationId Id,
         IEnumerable<string> tableNoValue,
         DateTimeOffset OccurredAt
    ) : IDomainEvent;
    
}
