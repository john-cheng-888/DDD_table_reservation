using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations.Event {
    public  record struct ReservationPlaced(
          ReservationId ReservationId,
          DateTimeOffset StartAt,
          int size,
          DateTimeOffset OccuredAt
    ) : IDomainEvent; 
    
}
