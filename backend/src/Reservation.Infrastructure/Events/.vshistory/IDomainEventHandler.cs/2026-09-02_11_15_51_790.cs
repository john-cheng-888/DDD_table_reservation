using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Infrastructure.Events {
    //為什麼不直接用「Task HandleAsync(IDomainEvent domainEvnet, CancellationToken ct = default);}」
    //因為日後會用到Type ---你會常遇到cast來cast去需要,與其日後更寫,不如現在先允許之後的Type推算之需.
    //參考「DomainEventDispatcher.cs」
    public  interface IDomainEventHandler<in TEvent> where TEvent:IDomainEvent {
        Task HandleAsync(TEvent domainEvnet, CancellationToken ct = default);
    }
}
