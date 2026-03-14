using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentWebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePracticeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shift_end_time",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "shift_start_time",
                table: "practices");

            migrationBuilder.RenameColumn(
                name: "street_number",
                table: "practices",
                newName: "place_id");

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "practices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "friday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "friday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "monday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "monday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "practices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "phone",
                table: "practices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "saturday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "saturday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "sunday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "sunday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "thursday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "thursday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "tuesday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "tuesday_start",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "wednesday_end",
                table: "practices",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "wednesday_start",
                table: "practices",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "friday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "friday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "monday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "monday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "name",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "saturday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "saturday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "sunday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "sunday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "thursday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "thursday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "tuesday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "tuesday_start",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "wednesday_end",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "wednesday_start",
                table: "practices");

            migrationBuilder.RenameColumn(
                name: "place_id",
                table: "practices",
                newName: "street_number");

            migrationBuilder.AddColumn<DateTime>(
                name: "shift_end_time",
                table: "practices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "shift_start_time",
                table: "practices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
