using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FullAddress1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appointment_duration",
                table: "practices");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "practices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "appointment_duration",
                table: "practices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "practices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
