using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservice.Customer.Address.Api.Migrations
{
    /// <inheritdoc />
    public partial class createpopulatetables : Migration
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
                    { new Guid("0a077be6-59a1-4768-9c38-722caba15c3f"), "15 Avocet Road", "Frodingly", "", 3, "South Glamorgan", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8461), new Guid("5ff79dfe-c1fa-4dd9-996f-bc96649d6dfc"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8463), "CF4 DET", "Cardiff" },
                    { new Guid("1f65b6b3-031f-4c63-8d56-c8730b0a579b"), "23 Long Shank Road", "Bordly", "Lower Horton", 1, "West Yorkshire", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8349), new Guid("aa1dc96f-3be5-41cd-8a1b-207284af3fdd"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8392), "HD6 TRF", "Horton" },
                    { new Guid("335565df-cadf-457a-b15e-f6d2f564fef5"), "18 Curlew Street", "Bardslow", "", 4, "County Londonderry", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8466), new Guid("ae55b0d1-ba02-41e1-9efa-9b4d4ac15eec"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8468), "DR4 GTY", "Derry" },
                    { new Guid("62aba6d4-5e5c-4566-8d46-2503ee78145a"), "3 Osprey Street", "Mineton", "Manely", 4, "County Antrim", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8443), new Guid("2385de72-2302-4ced-866a-fa199116ca6e"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8444), "BF2 PLD", "Belfast" },
                    { new Guid("86193bba-ab21-49d7-9686-05e1c67df6b3"), "1 Sparrow Road", "Halthorpe", "", 3, "West Glamorgan", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8438), new Guid("55b431ff-693e-4664-8f65-cfd8d0b14b1b"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8439), "SW4 NVD", "Swansea" },
                    { new Guid("a0c1a0ad-1d59-4951-a6cd-2d26951465fd"), "9 Short Eared Owl Lane", "Coorly", "", 2, "Stirlingshire", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8424), new Guid("af95fb7e-8d97-4892-8da3-5e6e51c54044"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8426), "ST4 VFR", "Stirling" },
                    { new Guid("a3c18b53-aabd-4ce8-bac9-1e3458356a32"), "21 Golden Eagle Way", "Plorton", "Riddleworth", 1, "West Yorkshire", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8471), new Guid("c95ba8ff-06a1-49d0-bc45-83f89b3ce820"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8472), "LS3 VFR", "Leeds" },
                    { new Guid("d0718d52-b5bc-4861-894b-a8f25b0ec1fb"), "66 Seagull Way", "Limestone", "", 1, "Northamptonshire", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8448), new Guid("47417642-87d9-4047-ae13-4c721d99ab48"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8450), "PE7 8TY", "Oundle" },
                    { new Guid("e3c4e1f9-8ba9-4cc3-ad0a-69528bb1d0f4"), "4 Buzzard Lane", "Needleton", "Harlslon", 2, "Aberdeenshire", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8457), new Guid("ff4d5a80-81e3-42e3-8052-92cf5c51e797"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8458), "AB3 DER", "Aberdeen" },
                    { new Guid("e6484ab8-801e-4270-8825-359610e83365"), "33 Blackbird Lane", "Reaverson", "", 1, "Durham", new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8477), new Guid("f07e88ac-53b2-4def-af07-957cbb18523c"), new DateTime(2024, 5, 7, 14, 47, 43, 71, DateTimeKind.Local).AddTicks(8478), "PL3 ABF", "Easington" }
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
