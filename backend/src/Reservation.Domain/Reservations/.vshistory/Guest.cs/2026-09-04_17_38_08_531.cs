using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class Guest:ValueObject {
       public string Name { get; }

       private Guest() { } //<--FOR ef init need.like "Reservation" class 
       public PhoneNumber PhoneNumber { get; }

        
        private Guest(string name, string phoneNumber) {
            //↑本來是「Guest(string name, string phoneNumber)」,
            //後來在「Reservation.Infrastructure.Persistence.Configurations」用的是「 g.OwnsOne(x => x.PhoneNumber)....」
            //所以要改為「Guest(string name, string phoneNumber)」
            if (name==null|| name.Trim().Length < 2) throw new DomainException($"客戶姓名太短,至少兩字以上,目前值為:{name}");
            Name = name;
            PhoneNumber = PhoneNumber.Of(phoneNumber);
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
