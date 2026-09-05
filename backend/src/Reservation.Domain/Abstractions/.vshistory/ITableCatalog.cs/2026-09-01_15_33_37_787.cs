using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface ITableCatalog {
        Task<IReadOnlyList<RestarurantTable>> GetAllAsync(CancellationToken ct = default);
    }
}
