using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class Guest:ValueObject {
       public string Name { get; }
       public PhoneNumber PhoneNumber { get; }

        private Guest(string name, string number) {
            if (name==null|| name.Trim().Length < 2) throw new DomainException($"客戶姓名太短,至少兩字以上,目前值為:{name}");
            Name = name;
            PhoneNumber = PhoneNumber.Of(number);
        }
        public static Guest Of(string name, string number) {
            return new Guest(name, number);
        }
        public override string ToString() {
            return $"{Name} {PhoneNumber.ToString()}";  //base.ToString();
        }

        protected override IEnumerable<object?> GetEqualityComponents() {
            //throw new NotImplementedException();
            yield return Name;
            yield return PhoneNumber;
        }
    }
}
