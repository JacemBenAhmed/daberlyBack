using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaberlyProjet.Migrations
{
    /// <inheritdoc />
    public partial class agent_vendeur_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentVendeurs",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "integer", nullable: false),
                    VendeurId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentVendeurs", x => new { x.AgentId, x.VendeurId });
                    table.ForeignKey(
                        name: "FK_AgentVendeurs_Users_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentVendeurs_Users_VendeurId",
                        column: x => x.VendeurId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentVendeurs_VendeurId",
                table: "AgentVendeurs",
                column: "VendeurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentVendeurs");
        }
    }
}
