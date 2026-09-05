using Reservation.Application.Reservations;

namespace Reservation.Api.Endpoints {
    public static class ReservationEndpoints {
        public record TestDto(string PersonName, string PhoneNunmber);
        public static void MapReservations(this WebApplication app) {
            var g = app.MapGroup("/api/reservations");
            g.MapPost("/", async (
                PlaceReservationCommand cmd,
                PlaceReservationHandler handler,
                CancellationToken ct) => {
                    var result = await handler.HandleAsync(cmd, ct);
                    return Results.Created($"/{result.ReservationId}", result);
                });
            g.MapPost("/TEST/",
                async (TestDto data) => {
                    return Results.Ok(new TestDto(data.PersonName.ToUpper(),data.PhoneNunmber.ToUpper()));

                    //return Results.Ok($"{helloTo.ToUpper()}");
                });
        }
    }
}
