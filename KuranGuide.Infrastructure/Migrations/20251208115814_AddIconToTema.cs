using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIconToTema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Temalar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Temalar");
        }
    }
}
