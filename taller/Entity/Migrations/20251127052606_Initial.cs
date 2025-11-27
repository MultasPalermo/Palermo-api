using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dueDayOfMonth",
                schema: "Parameters",
                table: "paymentFrequency",
                newName: "IntervalValue");

            migrationBuilder.AddColumn<string>(
                name: "IntervalType",
                schema: "Parameters",
                table: "paymentFrequency",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PasswordResetCodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isUsed = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetCodes", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "notificationSetting",
                keyColumn: "id",
                keyValue: 3,
                column: "Days",
                value: 80);

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "IntervalType", "IntervalValue" },
                values: new object[] { "Months", 1 });

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "IntervalType", "IntervalValue" },
                values: new object[] { "Days", 15 });

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "IntervalType", "IntervalValue" },
                values: new object[] { "Months", 2 });

            migrationBuilder.InsertData(
                schema: "Parameters",
                table: "paymentFrequency",
                columns: new[] { "id", "IntervalType", "IntervalValue", "active", "created_date", "intervalPage", "is_deleted" },
                values: new object[,]
                {
                    { 4, "Days", 7, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEMANAL", false },
                    { 5, "Months", 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TRIMESTRAL", false },
                    { 6, "Months", 6, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEMESTRAL", false },
                    { 7, "Years", 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ANUAL", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetCodes");

            migrationBuilder.DeleteData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "IntervalType",
                schema: "Parameters",
                table: "paymentFrequency");

            migrationBuilder.RenameColumn(
                name: "IntervalValue",
                schema: "Parameters",
                table: "paymentFrequency",
                newName: "dueDayOfMonth");

            migrationBuilder.UpdateData(
                table: "notificationSetting",
                keyColumn: "id",
                keyValue: 3,
                column: "Days",
                value: 60);

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 1,
                column: "dueDayOfMonth",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 2,
                column: "dueDayOfMonth",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Parameters",
                table: "paymentFrequency",
                keyColumn: "id",
                keyValue: 3,
                column: "dueDayOfMonth",
                value: 10);
        }
    }
}
