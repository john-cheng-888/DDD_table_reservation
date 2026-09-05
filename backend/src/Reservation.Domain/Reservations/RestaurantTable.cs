using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class RestaurantTable : ValueObject {
        public TableNo No { get; }
        public int Capacity { get; }
        private RestaurantTable(TableNo no, int capacity) {
            No = no;
            Capacity = capacity;
        }
        public static RestaurantTable Of(TableNo no, int capacity) {
            if (capacity < 1 || capacity > 20)
                throw new DomainException($"桌位容量須介於1~20,收到{capacity}");
            return new RestaurantTable(no, capacity);
        }
        protected override IEnumerable<object?> GetEqualityComponents() {
            //throw new NotImplementedException();
            yield return No;
            yield return Capacity;
        }
    }
}
