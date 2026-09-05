using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Common {
    //ref file P:\DddShop\src\DddShop.Domain\Orders
    public sealed class  ReservationId {
        private ReservationId() {
            value = Guid.NewGuid();
        }
        Guid value { get; }
        public override string ToString() => value.ToString();

        public static ReservationId New() {
            return new();
        }
    }
}
