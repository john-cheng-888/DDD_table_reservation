using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Reservation.Domain.Common {
    //ref file P:\DddShop\src\DddShop.Domain\Orders
    public readonly record struct ReservationId(Guid Value) {
        public static ReservationId New() => new ReservationId(Guid.NewGuid());
        public static ReservationId From(Guid value) =>
            value == Guid.Empty ?
                    throw new DomainException("ReservationId不可為空的Guid") :
                    new ReservationId(value);
        public override string ToString() => Value.ToString();
             
    }
    
    //以下錯誤示範
    /*
    public sealed  class  ReservationId {
        private ReservationId() {
            value = Guid.NewGuid();
        }
        Guid value { get; }
        public override string ToString() => value.ToString();

        public static ReservationId New() {
            return new();
        }
    }
    */
}
