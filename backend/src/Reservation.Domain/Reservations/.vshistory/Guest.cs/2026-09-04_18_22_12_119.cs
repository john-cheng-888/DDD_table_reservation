using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class Guest:ValueObject {
       public string Name { get; }

       
       public PhoneNumber PhoneNumber { get; }

       private Guest() { } //<--FOR ef init need.like "Reservation" class 
        //PhoneNumber 是 owned type navigation，不能綁到建構子參數，
        // 所以 Guest(string, string) 無法用於 materialization。

        // EF Core 專用。沒有這個建構子，model building 階段就會失敗：
        //   "No suitable constructor was found for the type 'Guest'"
        //
        // 原因：EF 會嘗試用建構子來 materialize，參數要能「按名稱」對應到已對應的屬性。
        //   name          → Name           ✓
        //   number        → 沒有 Number 屬性 ✗
        // 就算改名成 phoneNumber 也一樣不行 —— PhoneNumber 是 owned type navigation
        //   (見 ReservationConfiguration 的 g.OwnsOne(x => x.PhoneNumber))，
        //   而 navigation 不能綁到建構子參數。
        //
        // 所以唯一的解法是給 EF 一個無參數建構子，讓它直接寫入 backing field。
        // 跟 Reservation 那個 private Reservation() 是同一個理由。        


        private Guest(string name, string phoneNumber) {
            // 領域用的建構子。參數改名 number → phoneNumber 純粹是可讀性，
            // 與上面的 EF 問題無關。
            if (name==null|| name.Trim().Length < 2) throw new DomainException($"客戶姓名太短,至少兩字以上,目前值為:{name}");
            Name = name;
            PhoneNumber = PhoneNumber.Of(phoneNumber);
        }
        public static Guest Of(string name, string phoneNumber) {
            return new Guest(name, phoneNumber);
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
