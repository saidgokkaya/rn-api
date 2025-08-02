using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class malsaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mahalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mahalle_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Numarataj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BelgeNoId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TcKimlikNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mah = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaddeSokak = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisKapi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisKapi2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcKapiNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BagimsizBolge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EskiAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlokAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsYeriUnvani = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcKapiSayisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsyeriSayisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pafta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parsel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumaratajType = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    MahalleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numarataj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Numarataj_Mahalle_MahalleId",
                        column: x => x.MahalleId,
                        principalTable: "Mahalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Numarataj_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mahalle_OrganizationId",
                table: "Mahalle",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Numarataj_MahalleId",
                table: "Numarataj",
                column: "MahalleId");

            migrationBuilder.CreateIndex(
                name: "IX_Numarataj_OrganizationId",
                table: "Numarataj",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Numarataj");

            migrationBuilder.DropTable(
                name: "Mahalle");
        }
    }
}
