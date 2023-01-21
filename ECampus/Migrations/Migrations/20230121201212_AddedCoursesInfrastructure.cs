using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations;

/// <inheritdoc />
public partial class AddedCoursesInfrastructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                SubjectId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Courses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Courses_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CourseGroups",
            columns: table => new
            {
                CourseId = table.Column<int>(type: "int", nullable: false),
                GroupId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseGroups", x => new { x.CourseId, x.GroupId });
                table.ForeignKey(
                    name: "FK_CourseGroups_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourseGroups_Groups_GroupId",
                    column: x => x.GroupId,
                    principalTable: "Groups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourseTasks",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                ValidAfterDeadline = table.Column<bool>(type: "bit", nullable: false),
                MaxPoints = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                CourseId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseTasks", x => x.Id);
                table.ForeignKey(
                    name: "FK_CourseTasks_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CourseTeachers",
            columns: table => new
            {
                TeacherId = table.Column<int>(type: "int", nullable: false),
                CourseId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseTeachers", x => new { x.CourseId, x.TeacherId });
                table.ForeignKey(
                    name: "FK_CourseTeachers_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourseTeachers_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TaskSubmissions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                TotalPoints = table.Column<int>(type: "int", nullable: false),
                CourseTaskId = table.Column<int>(type: "int", nullable: false),
                StudentId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TaskSubmissions", x => x.Id);
                table.ForeignKey(
                    name: "FK_TaskSubmissions_CourseTasks_CourseTaskId",
                    column: x => x.CourseTaskId,
                    principalTable: "CourseTasks",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TaskSubmissions_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CourseGroups_GroupId",
            table: "CourseGroups",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_Courses_SubjectId",
            table: "Courses",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseTasks_CourseId",
            table: "CourseTasks",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseTeachers_TeacherId",
            table: "CourseTeachers",
            column: "TeacherId");

        migrationBuilder.CreateIndex(
            name: "IX_TaskSubmissions_CourseTaskId",
            table: "TaskSubmissions",
            column: "CourseTaskId");

        migrationBuilder.CreateIndex(
            name: "IX_TaskSubmissions_StudentId",
            table: "TaskSubmissions",
            column: "StudentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CourseGroups");

        migrationBuilder.DropTable(
            name: "CourseTeachers");

        migrationBuilder.DropTable(
            name: "TaskSubmissions");

        migrationBuilder.DropTable(
            name: "CourseTasks");

        migrationBuilder.DropTable(
            name: "Courses");
    }
}