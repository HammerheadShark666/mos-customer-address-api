using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservice.Customer.Address.Api.Migrations
{
    /// <inheritdoc />
    public partial class createtabledefaultdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MSOS_Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSOS_CustomerAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressLine3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TownCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSOS_CustomerAddress_MSOS_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MSOS_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MSOS_Country",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "England" },
                    { 2, "Scotland" },
                    { 3, "Wales" },
                    { 4, "Northern Ireland" }
                });

            migrationBuilder.InsertData(
                table: "MSOS_CustomerAddress",
                columns: new[] { "Id", "AddressLine1", "AddressLine2", "AddressLine3", "CountryId", "County", "Created", "CustomerId", "LastUpdated", "Postcode", "TownCity" },
                values: new object[,]
                {
                    { new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), "Intergration_Test", "Intergration_Test", "Intergration_Test", 1, "Intergration_Test", new DateTime(2024, 7, 27, 11, 46, 45, 590, DateTimeKind.Local).AddTicks(9928), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new DateTime(2024, 7, 27, 11, 46, 45, 590, DateTimeKind.Local).AddTicks(9976), "HD6 TRF", "Intergration_Test" },
                    { new Guid("b88ef4ce-739f-4c1b-b6d6-9d0727515de8"), "Intergration_Test2", "Intergration_Test2", "Intergration_Test2", 2, "Intergration_Test2", new DateTime(2024, 7, 27, 11, 46, 45, 591, DateTimeKind.Local).AddTicks(10), new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), new DateTime(2024, 7, 27, 11, 46, 45, 591, DateTimeKind.Local).AddTicks(12), "ST4 VFR", "Intergration_Test2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MSOS_CustomerAddress_CountryId",
                table: "MSOS_CustomerAddress",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSOS_CustomerAddress");

            migrationBuilder.DropTable(
                name: "MSOS_Country");
        }
    }
}
