using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Common {
    /// <summary>
    /// 讓 AppDbContext 能以非泛型方式蒐集領域事件。
    /// 沒有這個介面，DbContext 就得寫死 AggregateRoot<ReservationId>
    /// 之後新增第二個聚合時必須修改 DbContext。
    /// </summary>
    public interface IHasDomainEvents {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
