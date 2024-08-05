using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_voyage_log.Migrations
{
    /// <inheritdoc />
    public partial class VoyageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1L,
                column: "password_hash",
                value: "$2a$11$WUd0Yk/7.J12./At/qCBLedBV2JJXAXx41BS59aTuyZOD89eJDXt2");

            migrationBuilder.CreateIndex(
                name: "IX_voyages_countryFK",
                table: "voyages",
                column: "countryFK");

            migrationBuilder.CreateIndex(
                name: "IX_voyages_regionFK",
                table: "voyages",
                column: "regionFK");

            migrationBuilder.AddForeignKey(
                name: "FK_voyages_countries_countryFK",
                table: "voyages",
                column: "countryFK",
                principalTable: "countries",
                principalColumn: "country_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_voyages_regions_regionFK",
                table: "voyages",
                column: "regionFK",
                principalTable: "regions",
                principalColumn: "region_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voyages_countries_countryFK",
                table: "voyages");

            migrationBuilder.DropForeignKey(
                name: "FK_voyages_regions_regionFK",
                table: "voyages");

            migrationBuilder.DropIndex(
                name: "IX_voyages_countryFK",
                table: "voyages");

            migrationBuilder.DropIndex(
                name: "IX_voyages_regionFK",
                table: "voyages");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1L,
                column: "password_hash",
                value: "$2a$11$LWhhkffnmPNLGTuz4FZ3WettPfsVrsJm9LxkhtFZqRjusAm4BjdxW");
        }
    }
}
