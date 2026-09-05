using Microsoft.EntityFrameworkCore;
//以下的using如果發生錯誤(即便你已加入 project reference ,還是無法auto-complete,找不到路徑)
//請把該project unload再reload再加入 project reference 即可
using Reservation.Infrastructure.Allocation;
using Reservation.Infrastructure.Events;
using Reservation.Infrastructure.Events.Handlers;
using Reservation.Infrastructure.Persistence;
using Reservation.Domain.Abstractions;
using Reservation.Domain.Reservations.Event;
using Reservation.Api;


var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("default")
           ?? throw new InvalidOperationException("找不到Db Connection string");

//API:  AddDbContext<AppDbContext>(Action(optionBuilder));
//原文「(optionBuilder)」是沒有「()」,今為了好了解,故加上小括號
builder.Services.AddDbContext<AppDbContext>(
         (optionBuilder) =>{
                optionBuilder.UseSqlServer(conn, sql => sql.EnableRetryOnFailure(3));
                if (builder.Environment.IsDevelopment()) {
                 optionBuilder.EnableSensitiveDataLogging();//<--shows sql params/bindings
                 optionBuilder.EnableDetailedErrors();
                }
         }
);

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();//<--AppDbContext會用到
builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
//↑為什麼不寫作「builder.Services.AddScoped<IUnitOfWork,AppDbContext>();」 ???
//參考以下的片段
//public sealed class CreateReservationHandler(
//    IReservationRepository repository,   // ← got AppDbContext instance #1
//    IUnitOfWork unitOfWork)              // ← got AppDbContext instance #2
//
/*
用 GetRequiredService 而非 new，確保 Repository 與 UnitOfWork 拿到「同一個」 DbContext 實例。
用 new 的話 Repository 加的資料 SaveChanges 看不到。
*/

builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddSingleton<ITableCatalog,InMemoryTableCatalog>();
builder.Services.AddScoped<ITableAllocator, GreedyTableAllocator>();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddScoped<IDomainEventHandler<ReservationConfirmed>, LogReservationConfirmed>();
builder.Services.AddCors(o => o.AddDefaultPolicy(
      p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()
    ));

var app =builder.Build();
app.UseCors();
app.UseMiddleware<DomainExceptionMiddleware>();
app.MapGet("/", () => "Hello World!");
app.Run();
