using Reservation.Domain.Common;
using Reservation.Domain.Reservations.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public enum ReservationStatus { 
        待確認,//Suspend
        確認,//Confirmed
        入席,//Seated
        取消,//Cancel
        放鳥//NoShow
    }
    public sealed class Reservation:AggregateRoot<ReservationId> {
        public Guest Guest { get; private set; } = default!;
        public TimeSlot Slot { get; private set; } = default!;
        public PartySize Size { get; private set; } = default!;
        public TableNo? Table { get; private set; }
        public ReservationStatus Status { get; private set; }
        private Reservation() { }//for EF core.

        public static Reservation Place(Guest guest, TimeSlot slot, PartySize size, DateTimeOffset now) {
            if (slot.StartAt < now) {
                throw new DomainException("不可訂過去時段");
            }

            var r = new Reservation {
                //"Id" type as TID in AggregateRoot
                Id = ReservationId.New(),
                Guest=guest,
                Slot=slot,
                Size=size,
                Status=ReservationStatus.待確認
            };
            r.Raise(new ReservationPlaced(r.Id, slot.StartAt, size.Value, now));
            return r;
        }
        public void Confirm(TableNo table, DateTimeOffset now) {
            if (Status != ReservationStatus.待確認) {
                throw new DomainException($"只有 {nameof(ReservationStatus.待確認)}才可以進行確認,目前此單的狀態為{Status}");
            }
            Table = table;
            Status = ReservationStatus.確認;
            Raise(new ReservationConfirmed(Id,table.Value,now));
        }
        public void Cancel(string reason, DateTimeOffset now) {
            if (Status is ReservationStatus.取消 or ReservationStatus.入席) {
                throw new DomainException($"狀態為{Status},不可取消");
            }
            if (Slot.StartAt - now < TimeSpan.FromHours(2)) {
                throw new DomainException($"開席前不到2小時,不可線上取消,請致電餐廳");
            }
            Status = ReservationStatus.取消;
            Raise(new ReservationCancelled(Id,reason,now));

        }
           

    }


}
