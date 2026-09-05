using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Reservation.Domain.Reservations {
    /// <summary>
    /// 強型別 Id。用 readonly record struct 的理由：
    /// 1. 免費取得值相等（class 會是參考相等，Repository 比對與 Assert.Equal 都會失敗）
    /// 2. 不可為 null
    /// 3. EF Core 可透過 HasConversion 與 Guid 互轉
    /// </summary>    
    public readonly record struct ReservationId(Guid Value) {
        public static ReservationId New() => new ReservationId(Guid.NewGuid());

        /// <summary>還原一個既有身分（EF 從資料庫讀回時使用）。</summary>
        public static ReservationId From(Guid value) =>
            value == Guid.Empty ?
                    throw new InvalidateInputExcption("ReservationId不可為空的Guid") :
                    new ReservationId(value);

        public static bool TryParse(string? s, out ReservationId id) {
            id = default;
            if (!Guid.TryParse(s, out var guid) || guid == Guid.Empty) return false;
            id = new ReservationId(guid);
            return true;
        }
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
