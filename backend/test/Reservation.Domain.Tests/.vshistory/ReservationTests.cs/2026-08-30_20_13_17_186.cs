using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Reservations;
namespace Reservation.Domain.Tests {
    public class ReservationTests {
        private static DateTimeOffset Now = new DateTimeOffset(2026, 8, 30, 20, 0, 0, TimeSpan.FromHours(8));
        private static global::Reservation.Domain.Reservations.Reservation 建立有效訂位(int hours = 18)
            => global::Reservation.Domain.Reservations.Reservation.Place(
                  Guest.Of("王小明", "0912345678"),
                  TimeSlot.Of(
                     new DateTimeOffset(Now.Date.AddHours(hours), TimeSpan.FromHours(8)),
                     TimeSpan.FromMinutes(90)),
                  PartySize.Of(4),
                  Now);


    }
}
