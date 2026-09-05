using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Common;
namespace Reservation.Domain.Abstractions {
    public interface IDomainEventDispatcher {
        Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
    }
}
