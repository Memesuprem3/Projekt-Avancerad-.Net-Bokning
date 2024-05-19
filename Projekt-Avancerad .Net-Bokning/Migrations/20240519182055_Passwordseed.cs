using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_Avancerad_.Net_Bokning.Migrations
{
    /// <inheritdoc />
    public partial class Passwordseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52b38019-de6c-472b-bbca-db1fac587786", "AQAAAAIAAYagAAAAEJF3TKYlIg0l6JfXW0mcepIecDUb2TumTjgbJImI5QWKm5puncd8uUC8zFbmE3DU4g==", "62b9b37b-8ff2-4921-be0c-cfdb73a2c98e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3dc6b434-3eb4-4ae3-b5c3-109b93808832", "AQAAAAIAAYagAAAAEFTmg2xM0VNaIVNbek9FLgfl/a8Yo1aj/o6mjirkgQYjn53P3DMJwh2zMUtewE3onQ==", "7055bb82-505a-4b14-87c3-87faec06c2b0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "125c7ca3-c337-451a-b076-4791401e81d0", "AQAAAAIAAYagAAAAEL0XvafytK3Zh/nNdrs2f4CrvFQxhxuuKlmtYyy7RBXYN/i48zcvvEnX73HmNIWZlw==", "5ade3a3d-01f0-4aa4-a88f-542db2be32e4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "48508003-f049-42e7-aa62-f5446d96dc42", "AQAAAAIAAYagAAAAEFaRvvzNxukFc2U7RYzyW8Cxjotmz8TANy81X3oH03srdRRUxO9h2vs9QLkrgXGQZA==", "d3dfe351-8829-4de7-ab68-aef30aab31e2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17142352-0324-4826-a541-ebc96cd6b8b5", "AQAAAAIAAYagAAAAEMQ3KNJOzUFxv3GL3h2zXjHbYKqdiOSZf3JdMVBPp+0rr6GK3NqelfdMb5GrRBS8Vg==", "55f79278-e975-431f-9d27-85f13bc4a3e5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6447abc-cae3-468f-943e-daeac88f4ded", "AQAAAAIAAYagAAAAEMmOLTUk5P19p+Nj+RNY3BG4NSOCHUMvFsTgGiDouqGzQ706ZosFoS+JLciPpL9s+A==", "418cb19f-60e1-4a55-98e9-0703eef666f3" });
        }
    }
}
