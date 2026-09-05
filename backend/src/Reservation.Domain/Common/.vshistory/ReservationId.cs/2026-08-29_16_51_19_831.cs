using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Common {
    //ref file P:\DddShop\src\DddShop.Domain\Orders
    public readonly record struct ReservationId(Guid value) {
        public override string ToString() => value.ToString(); 
    }
}
