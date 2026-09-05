using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Events {
    public  interface IDomainEventHandler<in TEvent> where TEvent:IDomainEvent {
        Task HandleAsync(TEvent domainEvnet, CancellationToken ct = default);
    }
}
