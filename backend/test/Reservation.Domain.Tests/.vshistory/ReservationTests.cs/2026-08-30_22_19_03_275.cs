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
        private static DateTimeOffset Now = new DateTimeOffset(2026, 8, 30, 10, 0, 0, TimeSpan.FromHours(8));

        private static global::Reservation.Domain.Reservations.Reservation 建立有效訂位(int hours = 18) {
            var dateAfterHours = Now.AddHours(hours);
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
        public void 訂位時段超出營業時段_應拋出DomainException() {
            var start = Now.AddHours(22);
            Assert.Throws<DomainException>(()=>TimeSlot.Of(start,TimeSpan.FromMinutes(90)));
        }

        [Fact]
        public void 建立訂位_應產生訂位已建立事件() {
            var r = 建立有效訂位();
            Assert.Single(r.DomainEvents);
            Assert.IsType<ReservationPlaced>(r.DomainEvents.First());
        }
        [Fact]
        public void 開席前不足兩小時_不可取消訂位() {
            var r = 建立有效訂位(2);
            var 開席前一小時 = r.Slot.StartAt.AddHours(-1);
            var ex = Assert.Throws<DomainException>(()=>r.Cancel("雄雄有代志",開席前一小時));
            Assert.Contains("2小時", ex.Message);

        }
        [Fact]
        public void 已取消之訂位_不可再次確認() {
            var r = 建立有效訂位(3);
            r.Cancel("臨時有事", Now);
            Assert.Throws<DomainException>(()=>r.Confirm(TableNo.Of("X5"),Now));
        }
        [Fact]
        public void 時段相同的兩個TimeSlot應相等() {
            var start = Now.AddHours(5);
            Assert.Equal(
                 TimeSlot.Of(start, TimeSpan.FromMinutes(90)),
                 TimeSlot.Of(start, TimeSpan.FromMinutes(90))
                );
        }
    }
}
