using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DNMOFT.CountryOnMap.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<MultiPolygon>(
                name: "Coordenadas",
                table: "Provinces",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Provinces",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CountryId",
                table: "Provinces",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Countries_CountryId",
                table: "Provinces",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Countries_CountryId",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_CountryId",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Coordenadas",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Provinces");
        }
    }
}
