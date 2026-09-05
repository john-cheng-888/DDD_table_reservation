using Reservation.Domain.Common;
using Reservation.Domain.Reservations.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Reservation.Domain.Reservations {
    public enum ReservationStatus {
        [Description("待確認")] Pending,
        [Description("已確認")] Confirmed,
        [Description("已入座")] Seated,
        [Description("已取消")] Cancelled,
        [Description("未到")] NoShow,
    }
    public sealed class Reservation:AggregateRoot<ReservationId> {

        //允許多桌併桌
        private readonly List<TableNo> _tables = [];

        public Guest Guest { get; private set; } = default!;
        public TimeSlot Slot { get; private set; } = default!;
        public PartySize Size { get; private set; } = default!;
        //public TableNo? Table { get; private set; }
        private IReadOnlyCollection<TableNo> tables => _tables.AsReadOnly();
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
                Status=ReservationStatus.Pending
            };
            r.Raise(new ReservationPlaced(r.Id, slot.StartAt, size.Value, now));
            return r;
        }
        //public void Confirm(TableNo table, DateTimeOffset now) {
        public void Confirm(IEnumerable<TableNo>  tables, DateTimeOffset now) {
            if (Status != ReservationStatus.Pending) {
                throw new DomainException($"只有 {nameof(ReservationStatus.Pending)}才可以進行確認,目前此單的狀態為{Status}");
            }
            //Table = table;
            var assigned = (tables ?? []).Distinct().ToList();
            if (assigned.Count == 0)
                throw new DomainException("確認訂位時至少需配置一張桌位");

            _tables.Clear();
            _tables.AddRange(assigned);
            Status = ReservationStatus.Confirmed;
            //用ToList,而非本身直接給ReservationConfirmed的IReadOnlyList 參數,做 defensive copy,比較安全
            Raise(new ReservationConfirmed(Id,assigned.Select(t=>t.Value).ToList(),now));
        }

        //換桌
        public void Reassigne(IEnumerable<TableNo> tables, string reason, string , DateTimeOffset now) {
            if (Status is not (ReservationStatus.Confirmed or ReservationStatus.Seated)) {
                throw new DomainException($"狀態為{Status},不可換桌");
            }

        }

        public void Cancel(string reason, DateTimeOffset now) {
            if (Status is ReservationStatus.Cancelled or ReservationStatus.Seated or ReservationStatus.NoShow) {
                throw new DomainException($"目前狀態為{Status},不可取消");
            }
            if (Slot.StartAt - now < TimeSpan.FromHours(2)) {
                throw new DomainException($"開席前不到2小時,不可線上取消,請致電餐廳");
            }
            Status = ReservationStatus.Cancelled;
            Raise(new ReservationCancelled(Id,reason,now));
        }
        public void Seat(DateTimeOffset now) {
            if (Status != ReservationStatus.Confirmed) {
                throw new DomainException($"目前狀態為{Status},只有狀態為「{ReservationStatus.Confirmed}」才可入席");
            }
            Status = ReservationStatus.Seated;
            Raise(new GuestSeated(Id,now));
        }
        public void NoShow(DateTimeOffset now) {
            if (Status != ReservationStatus.Confirmed) {
                throw new DomainException($"目前狀態為{Status},只有狀態為「{ReservationStatus.Confirmed}」才可設為NoShow");
            }
            //只有 Confirmed 狀態且已超過開席時間 30 分鐘才能標記
            if (now - Slot.StartAt < TimeSpan.FromMinutes(30)) {
                throw new DomainException($"超過開席時間 30 分鐘才能標記NoShow,目前時尚差{(Slot.StartAt.AddMinutes(30)-now).ToString(@"hh\:mm")}");
            }
            Status = ReservationStatus.NoShow;
            Raise(new GuestNoShow(Id, now));

        }


    }


}
