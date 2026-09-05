using Reservation.Domain.Abstractions;
using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
namespace Reservation.Infrastructure.Events {
    public sealed class DomainEventDispatcher(IServiceProvider provider) : IDomainEventDispatcher {
        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default) {
            //throw new NotImplementedException();
            foreach (var e in events) {
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(e.GetType());
                var method = handlerType.GetMethod("HandleAsync");
                //only "registered" handler can handle it
                //otherwise pass
                foreach (var handler in provider.GetServices(handlerType)) {
                    if (handler != null) {
                        //呼叫「handler.HandleAsync」(傳入參數e & ct)
                        await (Task)method.Invoke(handler, [e, ct]);
                    }
                }
            }
        }
    }
}
