using Reservation.Application.Reservations;

namespace Reservation.Api.Endpoints {
    public static class ReservationEndpoints {
        //public record TestDto(string PersonName, string PhoneNunmber);
        public static void MapReservations(this WebApplication app) {
            var g = app.MapGroup("/api/reservations");
            /*how to post in post man to test?
             * Headers:not only "Content-type",but also other attr (Content-Length.....Connection) 
             * all need to check!!
             * body:
                    {
                       "guestName":"John Cheng",
                       "phone":"0932314448",
                       "startAt":"2026-09-10T18:00:00+08:00",
                       "durationMinutes":90,
                       "partySize":3
                    }             
             */
            g.MapPost("/", async (
                PlaceReservationCommand cmd,
                PlaceReservationHandler handler,
                CancellationToken ct) => {
                    var result = await handler.HandleAsync(cmd, ct);
                    return Results.Created($"/{result.ReservationId}", result);
                });

            //g.MapPost("/TEST/",
            //    async (TestDto data) => {
            //        return Results.Ok(new TestDto(data.PersonName.ToUpper(),data.PhoneNunmber.ToUpper()));
            //    });
        }
    }
}
