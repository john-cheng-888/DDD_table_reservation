using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Allocation {
    public sealed class InMemoryTableCatalog : ITableCatalog {

        // temporarily  use,in prod env,will move to database.
        private static readonly IReadOnlyList<RestaurantTable> Tables = [
              RestaurantTable.Of(TableNo.Of("A1"),2),
              RestaurantTable.Of(TableNo.Of("A2"),2),
              RestaurantTable.Of(TableNo.Of("B1"),4),
              RestaurantTable.Of(TableNo.Of("B2"),4),
              RestaurantTable.Of(TableNo.Of("C1"),8)
            ];

        public Task<IReadOnlyList<RestaurantTable>> GetAllAsync(CancellationToken ct = default) {
            //throw new NotImplementedException();
            return Task.FromResult(Tables);
        }
    }
}
