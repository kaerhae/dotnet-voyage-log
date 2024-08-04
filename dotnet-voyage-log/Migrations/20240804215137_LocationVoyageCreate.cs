using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace dotnet_voyage_log.Migrations
{
    /// <inheritdoc />
    public partial class LocationVoyageCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "voyages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "voyages",
                newName: "voyage_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "user_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "voyages",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<long>(
                name: "countryFK",
                table: "voyages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "regionFK",
                table: "voyages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    country_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    region_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    region_name = table.Column<string>(type: "text", nullable: false),
                    country_fk = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.region_id);
                    table.ForeignKey(
                        name: "FK_regions_countries_country_fk",
                        column: x => x.country_fk,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    city_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    city_name = table.Column<string>(type: "text", nullable: false),
                    region_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_cities_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "region_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1L,
                column: "password_hash",
                value: "$2a$11$eE6uV/eo6SovI/L2fIVYZewKjrd84u7GvmlZIJR7vI39za/LvaSum");

            migrationBuilder.CreateIndex(
                name: "IX_cities_region_id",
                table: "cities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_regions_country_fk",
                table: "regions",
                column: "country_fk");

            var countrySqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQL/bulk-insert-countries.sql");
            var sqlCountry = File.ReadAllText(countrySqlFilePath);
            migrationBuilder.Sql(sqlCountry);

            var regionSqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQL/bulk-insert-regions.sql");
            var sqlRegion = File.ReadAllText(regionSqlFilePath);
            migrationBuilder.Sql(sqlRegion);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropColumn(
                name: "countryFK",
                table: "voyages");

            migrationBuilder.DropColumn(
                name: "regionFK",
                table: "voyages");

            migrationBuilder.RenameColumn(
                name: "voyage_id",
                table: "voyages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "users",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "voyages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "voyages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "password_hash",
                value: "$2a$11$BgvE3Upie2ztH3yd9v6aee6kaLU7mhvS/iWetH1MZICz6MRjvty/6");
        
            migrationBuilder.Sql("DELETE FROM countries");
            migrationBuilder.Sql("DELETE FROM countries");
        }
    }
}
