using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EelegantIot.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOnToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_on",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_on",
                table: "Devices");
        }
    }
}
