using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenamedFacultacyIdToFacultyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacultacyId",
                table: "Departments",
                newName: "FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_FacultacyId",
                table: "Departments",
                newName: "IX_Departments_FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Departments",
                newName: "FacultacyId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                newName: "IX_Departments_FacultacyId");
        }
    }
}
