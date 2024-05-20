using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EelegantIot.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDayOfWeeks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "friday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "monday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "saturday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "sunday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "thursday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "tuesday",
                table: "device");

            migrationBuilder.DropColumn(
                name: "wednesday",
                table: "device");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeeks",
                table: "device",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeeks",
                table: "device");

            migrationBuilder.AddColumn<bool>(
                name: "friday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "monday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "saturday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "sunday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "thursday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "tuesday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "wednesday",
                table: "device",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
