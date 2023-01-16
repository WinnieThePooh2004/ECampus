using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations;

/// <inheritdoc />
public partial class UserCanBeGeristeredAsSpecificStudentOrTeacher : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "StudentId",
            table: "Users",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "TeacherId",
            table: "Users",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "Teachers",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "Students",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Teachers_UserId",
            table: "Teachers",
            column: "UserId",
            unique: true,
            filter: "[UserId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Students_UserId",
            table: "Students",
            column: "UserId",
            unique: true,
            filter: "[UserId] IS NOT NULL");
        
        migrationBuilder.CreateIndex(
            name: "IX_Users_TeacherId",
            table: "Users",
            column: "TeacherId",
            unique: true,
            filter: "[TeacherId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Users_StudentId",
            table: "Users",
            column: "StudentId",
            unique: true,
            filter: "[StudentId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Students_Users_UserId",
            table: "Students",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull,
            onUpdate: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_Teachers_Users_UserId",
            table: "Teachers",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull,
            onUpdate: ReferentialAction.SetNull);
        
        migrationBuilder.AddForeignKey(
            name: "FK_Users_Students_StudentId",
            table: "Users",
            column: "StudentId",
            principalTable: "Students",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction,
            onUpdate: ReferentialAction.NoAction);

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Teachers_TeacherId",
            table: "Users",
            column: "TeacherId",
            principalTable: "Teachers",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction,
            onUpdate: ReferentialAction.NoAction);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Students_Users_UserId",
            table: "Students");

        migrationBuilder.DropForeignKey(
            name: "FK_Teachers_Users_UserId",
            table: "Teachers");

        migrationBuilder.DropIndex(
            name: "IX_Teachers_UserId",
            table: "Teachers");

        migrationBuilder.DropIndex(
            name: "IX_Students_UserId",
            table: "Students");

        migrationBuilder.DropColumn(
            name: "StudentId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "TeacherId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Teachers");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Students");
    }
}