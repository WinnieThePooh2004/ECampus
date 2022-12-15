using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenamedFacultaciesToFaculties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Facultacies, Faculties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Faculties_FacultacyId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.CreateTable(
                name: "Facultacies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facultacies", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Facultacies_FacultacyId",
                table: "Departments",
                column: "FacultacyId",
                principalTable: "Facultacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
