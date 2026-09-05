using Microsoft.EntityFrameworkCore;
//以下的using如果發生錯誤(即便你已加入 project reference ,還是無法auto-complete,找不到路徑)
//請把該project unload再reload再加入 project reference 即可
using Reservation.Infrastructure.Allocation;
using Reservation.Infrastructure.Events;
using Reservation.Infrastructure.Events.Handlers;
using Reservation.Infrastructure.Persistence;
using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations.Event;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
