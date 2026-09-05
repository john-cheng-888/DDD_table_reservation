using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Reservation.Domain.Reservations.Event;
namespace Reservation.Infrastructure.Events.Handlers {
    public sealed class LogReservationConfirmed(ILogger<LogReservationConfirmed> logger) :
        IDomainEventHandler<ReservationConfirmed> {
        public Task HandleAsync(ReservationConfirmed e, CancellationToken ct = default) {
            //throw new NotImplementedException();
            logger.LogInformation("訂位 {Id} 已確認,桌號 {Tables}",e.Id,string.Join(",",e.tableNos));
            return Task.CompletedTask;
        }
    }
}
