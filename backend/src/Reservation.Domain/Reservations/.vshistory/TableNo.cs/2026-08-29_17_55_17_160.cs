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
            return new TableNo(value);
        }

        protected override IEnumerable<object?> GetEqualityComponents() {
            yield return Value;
        }
    }
}
