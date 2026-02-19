using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSureNoColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SureNo",
                table: "Ayetler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SureNo",
                table: "Ayetler",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
