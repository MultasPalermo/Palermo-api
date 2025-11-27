using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfractionPaymentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "smldvValueAtCreation",
                schema: "Entities",
                table: "userInfraction",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentDue3Days",
                schema: "Entities",
                table: "userInfraction",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentDue15Days",
                schema: "Entities",
                table: "userInfraction",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentDue25Days",
                schema: "Entities",
                table: "userInfraction",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCollection",
                schema: "Entities",
                table: "userInfraction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "smldvValueAtCreation",
                schema: "Entities",
                table: "userInfraction");

            migrationBuilder.DropColumn(
                name: "paymentDue3Days",
                schema: "Entities",
                table: "userInfraction");

            migrationBuilder.DropColumn(
                name: "paymentDue15Days",
                schema: "Entities",
                table: "userInfraction");

            migrationBuilder.DropColumn(
                name: "paymentDue25Days",
                schema: "Entities",
                table: "userInfraction");

            migrationBuilder.DropColumn(
                name: "StatusCollection",
                schema: "Entities",
                table: "userInfraction");
        }
    }
}
