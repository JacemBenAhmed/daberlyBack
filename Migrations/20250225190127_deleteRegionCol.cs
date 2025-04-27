using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaberlyProjet.Migrations
{
    /// <inheritdoc />
    public partial class deleteRegionCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "region",
                table: "Annonces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
