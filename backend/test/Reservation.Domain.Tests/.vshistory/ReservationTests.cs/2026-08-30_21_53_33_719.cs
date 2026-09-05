using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Reservation.Domain.Common;
using Reservation.Domain.Reservations;
using Reservation.Domain.Reservations.Event;
using Xunit; 
namespace Reservation.Domain.Tests {
    public class ReservationTests {
        private static DateTimeOffset Now = new DateTimeOffset(2026, 8, 30, 20, 0, 0, TimeSpan.FromHours(8));

        private static global::Reservation.Domain.Reservations.Reservation 建立有效訂位(int hours = 18) {
            var dateAfterHours = new DateTimeOffset(Now.Date.AddHours(hours), TimeSpan.FromHours(8));
            return global::Reservation.Domain.Reservations.Reservation.Place(
                      Guest.Of("王小明", "0912345678"),
                      TimeSlot.Of(dateAfterHours,TimeSpan.FromMinutes(90)),
                      PartySize.Of(4),
                      Now);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void MustThrowsDomainException_WhenPartySizeOutOfRange(int size) {
            var act = () => PartySize.Of(size);
            Assert.Throws<DomainException>(act);
        }
        [Fact]
        public void 建立訂位_應產生訂位已建立事件() {
            var r = 建立有效訂位();
            Assert.Single(r.DomainEvents);
            Assert.IsType<ReservationPlaced>(r.DomainEvents.First());
        }
    }
}
