using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Reservations;
namespace Reservation.Domain.Abstractions {
    public  interface IReservationRepository {
        Task<Reservation.Domain.Reservations.Reservation?> FindAsync(ReservationId id, CancellationToken ct = default);
        Task<IReadOnlyList<Reservation.Domain.Reservations.Reservation>>
            FindByDateAsync(DateOnly date, CancellationToken ct = default);
        Task AddAsync(Reservation.Domain.Reservations.Reservation reservation,CancellationToken ct=default);
    }
}
