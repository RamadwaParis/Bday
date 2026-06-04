using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirthdayApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByToBirthdays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Birthdays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Birthdays");
        }
    }
}
