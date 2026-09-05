using Microsoft.AspNetCore.Http.HttpResults;
using Reservation.Application.Reservations;
using Microsoft.AspNetCore.Mvc;
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
            //Map裡,所有的handler都是要在program.cs的service有註冊才能用

            g.MapPost("/", async (
                PlaceReservationCommand cmd,
                PlaceReservationHandler handler,
                CancellationToken ct) => {
                    var result = await handler.HandleAsync(cmd, ct);
                    return Results.Created($"/{result.ReservationId}", result);
                });
            
            g.MapGet("/{id}/detail",
                  async (
                        Guid id,
                        QueryReservationHandler handler,
                        CancellationToken ct
                      ) => {
                          
                          var result = await handler.FindAsync(id,ct);
                          return result==null?Results.NotFound():Results.Json(result, statusCode:200);
                      }
             );
            

            //g.MapPost("/TEST/",
            //    async (TestDto data) => {
            //        return Results.Ok(new TestDto(data.PersonName.ToUpper(),data.PhoneNunmber.ToUpper()));
            //    });
        }
    }
}
