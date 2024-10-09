using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSalon.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeGeneratedTime",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "EmailCode",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "LoginTypeEnum",
                table: "ApplicationUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CodeGeneratedTime",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailCode",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginTypeEnum",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
