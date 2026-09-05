using Reservation.Domain.Reservations;
using Reservation.Domain.Common;
namespace Reservation.Domain.Abstractions {
    public  interface IReservationRepository {
        Task<Reservations.Reservation?> FindAsync(ReservationId id, CancellationToken ct = default);
        Task<IReadOnlyList<Reservations.Reservation>> FindByDateAsync(DateOnly date, CancellationToken ct = default);
        Task AddAsync(Reservations.Reservation reservation,CancellationToken ct=default);
    }
}
