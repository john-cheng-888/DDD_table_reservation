using Azure;
using Microsoft.AspNetCore.Mvc;
using Reservation.Domain.Common;
namespace Reservation.Api {
    public sealed class DomainExceptionMiddleware(RequestDelegate next) {
        public async Task InvokeAsync(HttpContext ctx) {
            //----------------------注意 ↑是「HttpContext」,不是「HttpContent」!!
            try {
                await next(ctx);
            } catch (BusinessRuleViolationException ex) {
                await WriteAsync(ctx, StatusCodes.Status409Conflict, "違反業務規則", ex.Message); 

            } catch (InvalidateInputExcption ex) {
                await WriteAsync(ctx, StatusCodes.Status400BadRequest, "請求不合法", ex.Message);

            } catch (DomainException ex) {
                await WriteAsync(ctx, StatusCodes.Status400BadRequest, "請求不合法", ex.Message);
            }
        }
        private static Task WriteAsync(HttpContext ctx, int status, string title, string detail) {
            ctx.Response.StatusCode = status;
            return ctx.Response.WriteAsJsonAsync(
                 new ProblemDetails {
                     Status = status,
                     Detail = detail,
                     Title = title
                 }
            );
        }
    }
}
