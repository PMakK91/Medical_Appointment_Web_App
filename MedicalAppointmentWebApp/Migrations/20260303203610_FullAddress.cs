using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FullAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "practices");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "practices",
                newName: "full_address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "full_address",
                table: "practices",
                newName: "street");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "practices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
