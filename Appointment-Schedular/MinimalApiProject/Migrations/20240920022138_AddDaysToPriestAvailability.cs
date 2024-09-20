using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiProject.Migrations
{
    /// <inheritdoc />
    public partial class AddDaysToPriestAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PriestAvailabilities",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "Days",
                table: "PriestAvailabilities",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PriestAvailabilities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "PriestAvailabilities");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PriestAvailabilities");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "PriestAvailabilities",
                newName: "Date");
        }
    }
}
