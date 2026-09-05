using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Allocation {
    public sealed class InMemoryTableCatalog : ITableCatalog {
        private static readonly IReadOnlyList<RestarurantTable> Tables = [
              RestarurantTable.Of(TableNo.Of("A1"),2),
              RestarurantTable.Of(TableNo.Of("A2"),2),
              RestarurantTable.Of(TableNo.Of("B1"),4),
              RestarurantTable.Of(TableNo.Of("B2"),4),
              RestarurantTable.Of(TableNo.Of("C1"),8)
            ];

        public Task<IReadOnlyList<RestarurantTable>> GetAllAsync(CancellationToken ct = default) {
            //throw new NotImplementedException();
            return Task.FromResult(Tables);
        }
    }
}
