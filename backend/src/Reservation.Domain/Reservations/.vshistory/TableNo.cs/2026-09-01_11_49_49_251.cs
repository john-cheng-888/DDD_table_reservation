using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class TableNo:ValueObject {
        public string Value { get; }
        private TableNo(string value) {
            Value = value;
        }
        public static TableNo Of(String value) {
            var v = value?.Trim().ToUpperInvariant() ?? "";
            if (v.Length == 0) throw new DomainException("桌號不可為空");
            if(v.Length>10) throw new DomainException($"桌號不可超過10個字元,目前收到的值為:{value}");
            return new TableNo(v);
        }

        protected override IEnumerable<object?> GetEqualityComponents() {
            yield return Value;
        }
    }
}
