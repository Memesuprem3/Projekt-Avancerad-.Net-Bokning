using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Projekt_Avancerad_.Net_Bokning.Migrations
{
    /// <inheritdoc />
    public partial class FirstSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FristName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointDiscription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlacedApp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointments_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "CompanyId", "CompanyName" },
                values: new object[,]
                {
                    { 1, "SaabParts" },
                    { 2, "VoloParts" },
                    { 3, "Macken" }
                });

            migrationBuilder.InsertData(
                table: "customer",
                columns: new[] { "CustomerId", "Adress", "Email", "FristName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "1234 Main St", "annaecool@hotmail.com", "Anna", "Svensson", "123-456-7890" },
                    { 2, "Vivolvägen 12", "R41nFire@hotmail.com", "Jonas", "Hellqvist", "7778889932" },
                    { 3, "Hästhagen 3", "bilarebra@hotmail.com", "Stefan", "Magnusson", "7778889932" },
                    { 4, "Hagelbrakare 41", "fettmedraggarvalle@hotmail.com", "Ronny", "Ronnysson", "7778889932" },
                    { 5, "Hagelbrakare 42", "fettmedraggarvallE@hotmail.com", "Ragge", "Raggesson", "7778889932" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "id", "AppointDiscription", "CompanyId", "CustomerId", "PlacedApp" },
                values: new object[,]
                {
                    { 1, "Initial Consultation", 1, 1, new DateTime(2011, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Second Consultation", 1, 1, new DateTime(2011, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Third Consultation", 1, 1, new DateTime(2011, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Initial Consultation", 2, 2, new DateTime(2011, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CompanyId",
                table: "Appointments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
