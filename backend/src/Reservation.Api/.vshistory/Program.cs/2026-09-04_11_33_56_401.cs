using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Abstractions;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
