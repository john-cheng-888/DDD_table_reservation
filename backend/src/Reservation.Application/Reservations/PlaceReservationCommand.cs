using Reservation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Reservation.Domain.Reservations;
using ReservationAggregate = Reservation.Domain.Reservations.Reservation;
using Reservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Reservation.Application.Reservations {
    public sealed record PlaceReservationCommand(
          string GuestName,
          string Phone,
          DateTimeOffset StartAt,
          int DurationMinutes,
          int PartySize,
          string? Note
     );

    public sealed record PlaceReservationResult(Guid ReservationId, string status, string[]? TableNo);

    public sealed class PlaceReservationHandler(
        IReservationRepository repository,
        ITableAllocator allocator,
        IUnitOfWork unitOfWork,
        //即然在Reservation.Api 的Program.cs裡有TimeProvider,就拿來用吧,這在debug--回溯時間場景時會很好用的
        TimeProvider timeProvider
        ) {
        public async Task<PlaceReservationResult> HandleAsync(PlaceReservationCommand cmd,CancellationToken ct=default) {
            var now = timeProvider.GetUtcNow();
            var rs = ReservationAggregate.Place(
                     Guest.Of(cmd.GuestName, cmd.Phone),
                     TimeSlot.Of(cmd.StartAt, TimeSpan.FromMinutes(cmd.DurationMinutes)),
                     PartySize.Of(cmd.PartySize),
                     now,cmd.Note);
            await repository.AddAsync(rs, ct);
            //allocation tableNo
            var tables = await allocator.AllocateAsync(rs.Slot, rs.Size, rs.Id, ct);
            if (tables!=null) {
                rs.Confirm(tables, now);
            }
            await  unitOfWork.SaveChangesAsync(ct);
            //to create"record" instance,only can do  by constructor, not by "{a=...,b=....c=....}"
            return new PlaceReservationResult ( 
                rs.Id.Value,
                rs.Status.ToString(),
                tables?.Select(r=>r.Value).ToArray()
            );
        }
    }
}
