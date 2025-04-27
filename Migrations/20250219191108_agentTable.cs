using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaberlyProjet.Migrations
{
    /// <inheritdoc />
    public partial class agentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentVendeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AgentId = table.Column<int>(type: "integer", nullable: false),
                    VendeurId = table.Column<int>(type: "integer", nullable: false)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentVendeurs", x => new { x.Id,x.AgentId, x.VendeurId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentVendeurs");
        }
    }
}
