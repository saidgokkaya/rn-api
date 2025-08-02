using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class malsaaaas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BagimsizBolge",
                table: "Numarataj");

            migrationBuilder.DropColumn(
                name: "BelgeNoId",
                table: "Numarataj");

            migrationBuilder.DropColumn(
                name: "IcKapiSayisi",
                table: "Numarataj");

            migrationBuilder.DropColumn(
                name: "IsyeriSayisi",
                table: "Numarataj");

            migrationBuilder.DropColumn(
                name: "Pafta",
                table: "Numarataj");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Numarataj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BagimsizBolge",
                table: "Numarataj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BelgeNoId",
                table: "Numarataj",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IcKapiSayisi",
                table: "Numarataj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsyeriSayisi",
                table: "Numarataj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pafta",
                table: "Numarataj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Numarataj",
                type: "datetime2",
                nullable: true);
        }
    }
}
