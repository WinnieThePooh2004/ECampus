using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AdddedUniqueIndexToTaskSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskSubmissions_StudentId",
                table: "TaskSubmissions");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSubmissions_StudentId_CourseTaskId",
                table: "TaskSubmissions",
                columns: new[] { "StudentId", "CourseTaskId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskSubmissions_StudentId_CourseTaskId",
                table: "TaskSubmissions");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSubmissions_StudentId",
                table: "TaskSubmissions",
                column: "StudentId");
        }
    }
}
