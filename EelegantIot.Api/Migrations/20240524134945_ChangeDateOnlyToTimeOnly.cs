using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EelegantIot.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateOnlyToTimeOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "end_at",
                table: "device",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "start_at",
                table: "device",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_at",
                table: "device");

            migrationBuilder.DropColumn(
                name: "start_at",
                table: "device");
        }
    }
}
