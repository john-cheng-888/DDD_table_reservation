using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuestPhone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PartySize = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationTables",
                columns: table => new
                {
                    TableNo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTables", x => new { x.ReservationId, x.TableNo });
                    table.ForeignKey(
                        name: "FK_ReservationTables_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_StartAt",
                table: "Reservations",
                column: "StartAt");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Status",
                table: "Reservations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TableNo",
                table: "ReservationTables",
                column: "TableNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationTables");

            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
