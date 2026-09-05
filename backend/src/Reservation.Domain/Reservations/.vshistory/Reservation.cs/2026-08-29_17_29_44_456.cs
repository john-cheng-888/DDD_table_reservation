using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public enum ReservationStatus { 
        未定,//Suspend
        確認,//Confirmed
        入席,//Seated
        取消,//Cancel
        放鳥//NoShow
    }
    public sealed class Reservation:AggregateRoot<ReservationId> {
        public Guest Guest { get; private set; } = default!;
        public TimeSlot Slot { get; private set; } = default!;



    }
}
