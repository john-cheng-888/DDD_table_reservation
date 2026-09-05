using Reservation.Application.Reservations;

namespace Reservation.Api.Endpoints {
    public static class ReservationEndpoints {
        public static void MapReservations(this WebApplication app) {
            var g = app.MapGroup("/api/reservations");
            g.MapPost("/", async (
                PlaceReservationCommand cmd,
                PlaceReservationHandler handler,
                CancellationToken ct) => {
                    var result = await handler.HandleAsync(cmd, ct);
                    return Results.Created($"/{result.ReservationId}", result);
                });
        }
    }
}
