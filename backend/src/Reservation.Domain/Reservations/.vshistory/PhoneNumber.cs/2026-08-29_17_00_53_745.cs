using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class PhoneNumber:ValueObject {
        public string Number { get; }
        private PhoneNumber(string number) {
            Number = number;
        }
        public static PhoneNumber Of(string number) {
            return new PhoneNumber(number);
        }

        public override string ToString() => $"電話:{Number}";
        protected override IEnumerable<object?> GetEqualityComponents() {
            //throw new NotImplementedException();
            yield return Number;
        }
    }
}
