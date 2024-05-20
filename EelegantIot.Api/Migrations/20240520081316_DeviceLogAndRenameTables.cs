using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EelegantIot.Api.Migrations
{
    /// <inheritdoc />
    public partial class DeviceLogAndRenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Users_UserId",
                table: "UserDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "UserDevices",
                newName: "user_devices");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "device");

            migrationBuilder.RenameIndex(
                name: "IX_UserDevices_DeviceId",
                table: "user_devices",
                newName: "IX_user_devices_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_devices",
                table: "user_devices",
                columns: new[] { "UserId", "DeviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_device",
                table: "device",
                column: "id");

            migrationBuilder.CreateTable(
                name: "device_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    humidity = table.Column<double>(type: "float", nullable: false),
                    temperature = table.Column<double>(type: "float", nullable: false),
                    current = table.Column<double>(type: "float", nullable: false),
                    voltage = table.Column<double>(type: "float", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_log", x => x.id);
                    table.ForeignKey(
                        name: "FK_device_log_device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "device",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_log_DeviceId",
                table: "device_log",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_devices_device_DeviceId",
                table: "user_devices",
                column: "DeviceId",
                principalTable: "device",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_devices_users_UserId",
                table: "user_devices",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_devices_device_DeviceId",
                table: "user_devices");

            migrationBuilder.DropForeignKey(
                name: "FK_user_devices_users_UserId",
                table: "user_devices");

            migrationBuilder.DropTable(
                name: "device_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_devices",
                table: "user_devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_device",
                table: "device");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "user_devices",
                newName: "UserDevices");

            migrationBuilder.RenameTable(
                name: "device",
                newName: "Devices");

            migrationBuilder.RenameIndex(
                name: "IX_user_devices_DeviceId",
                table: "UserDevices",
                newName: "IX_UserDevices_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices",
                columns: new[] { "UserId", "DeviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Users_UserId",
                table: "UserDevices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
