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
            if (((number.Trim().Length != 10) || (!number.StartsWith("09"))) && 
                  (!number.Any(x=>"0123456789".Contains(x)))
                ) {
                throw new DomainException("手機號碼格式錯誤,需為09開頭且長度為10碼數字");
            }
            return new PhoneNumber(number);
        }

        public override string ToString() => $"電話:{Number}";
        protected override IEnumerable<object?> GetEqualityComponents() {
            //throw new NotImplementedException();
            yield return Number;
        }
    }
}
