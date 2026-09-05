using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Common {
    //internal class ReservationId {
    //}
    public readonly record struct ReservationId(Guid value) {
        public override string ToString() => value.ToString(); 
    }
}
