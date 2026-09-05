using Reservation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Application.Reservations {
    public sealed record PlaceReservationCommand(
          string GustName,
          string Phone,
          DateTimeOffset StartAt,
          int DurationMinutes,
          int PartySize
     );

    public sealed record PlaceReservationResult(Guid ReservationId, string status, string? TableNo);

    public sealed class PlaceReservationHandler(
          IReservationRepository repository,
          ITableAllocator allocator

        ) { 
    }
}
