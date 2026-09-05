using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface ITableCatalog {
        Task<IReadOnlyList<RestaurantTable>> GetAllAsync(CancellationToken ct = default);
    }
}
