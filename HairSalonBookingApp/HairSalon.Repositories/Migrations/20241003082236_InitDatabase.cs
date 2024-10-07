using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSalon.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ServiceAppointments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "ServiceAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ServiceAppointments");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "ServiceAppointments");
        }
    }
}
