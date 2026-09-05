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

        // Factory Method：唯一的建立入口，確保物件誕生時就是有效的
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
                throw new DomainException($"目前狀態為{Status},不可取消");
            }
            if (Slot.StartAt - now < TimeSpan.FromHours(2)) {
                throw new DomainException($"開席前不到2小時,不可線上取消,請致電餐廳");
            }
            Status = ReservationStatus.取消;
            Raise(new ReservationCancelled(Id,reason,now));
        }
        public void Seat(DateTimeOffset now) {
            if (Status != ReservationStatus.確認) {
                throw new DomainException($"目前狀態為{Status},只有狀態為「{ReservationStatus.確認}」才可入席");
            }
            Status = ReservationStatus.入席;
            Raise(new GuestSeated(Id,now));
        }
        public void NoShow(DateTimeOffset now) {
            if (Status != ReservationStatus.確認) {
                throw new DomainException($"目前狀態為{Status},只有狀態為「{ReservationStatus.確認}」才可設為NoShow");
            }
            //只有 Confirmed 狀態且已超過開席時間 30 分鐘才能標記
            if (now - Slot.StartAt < TimeSpan.FromMinutes(30)) {
                throw new DomainException($"超過開席時間 30 分鐘才能標記NoShow,目前時差{(now - Slot.StartAt).Minutes}");
            }
            Status = ReservationStatus.放鳥;
            Raise(new GuestNoShow(Id, now));

        }


    }


}
