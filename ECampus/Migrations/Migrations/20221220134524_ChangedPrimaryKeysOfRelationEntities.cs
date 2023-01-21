using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPrimaryKeysOfRelationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTeachers",
                table: "UserTeachers");

            migrationBuilder.DropIndex(
                name: "IX_UserTeachers_UserId",
                table: "UserTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuditories",
                table: "UserAuditories");

            migrationBuilder.DropIndex(
                name: "IX_UserAuditories_UserId",
                table: "UserAuditories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_TeacherId",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTeachers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAuditories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SubjectTeachers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTeachers",
                table: "UserTeachers",
                columns: new[] { "UserId", "TeacherId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuditories",
                table: "UserAuditories",
                columns: new[] { "UserId", "AuditoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers",
                columns: new[] { "TeacherId", "SubjectId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTeachers",
                table: "UserTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuditories",
                table: "UserAuditories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserTeachers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserGroups",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserAuditories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SubjectTeachers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTeachers",
                table: "UserTeachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuditories",
                table: "UserAuditories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeachers_UserId",
                table: "UserTeachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuditories_UserId",
                table: "UserAuditories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_TeacherId",
                table: "SubjectTeachers",
                column: "TeacherId");
        }
    }
}
