using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppointmentWebApp.Migrations
{
    /// <inheritdoc />
    public partial class PasswordResetMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.token);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_user_id",
                table: "PasswordResetTokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetTokens");
        }
    }
}
