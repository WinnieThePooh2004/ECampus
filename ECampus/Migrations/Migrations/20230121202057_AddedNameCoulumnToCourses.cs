﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations;

/// <inheritdoc />
public partial class AddedNameCoulumnToCourses : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Courses",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "Courses");
    }
}