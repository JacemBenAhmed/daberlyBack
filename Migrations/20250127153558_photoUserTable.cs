using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DaberlyProjet.Migrations
{
    /// <inheritdoc />
    public partial class photoUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "fCIN",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "bCIN",
                table: "Users",
                newName: "Addresse");

            migrationBuilder.AlterColumn<string>(
                name: "numCin",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "PhotosUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    FCin = table.Column<string>(type: "text", nullable: false),
                    BCin = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotosUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotosUser_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotosUser_IdUser",
                table: "PhotosUser",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotosUser");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "fCIN");

            migrationBuilder.RenameColumn(
                name: "Addresse",
                table: "Users",
                newName: "bCIN");

            migrationBuilder.AlterColumn<int>(
                name: "numCin",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
