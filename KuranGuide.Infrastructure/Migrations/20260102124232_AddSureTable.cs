using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ayetler_Temalar_TemaId",
                table: "Ayetler");

            migrationBuilder.AlterColumn<int>(
                name: "TemaId",
                table: "Ayetler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Ayetler",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2147483647);

            migrationBuilder.AddColumn<int>(
                name: "SureId",
                table: "Ayetler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sureler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SureNo = table.Column<int>(type: "int", nullable: false),
                    SureAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArapcaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InisSirasi = table.Column<int>(type: "int", nullable: false),
                    AyetSayisi = table.Column<int>(type: "int", nullable: false),
                    CuzNo = table.Column<int>(type: "int", nullable: false),
                    SayfaNo = table.Column<int>(type: "int", nullable: false),
                    Yer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sureler", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ayetler_SureId",
                table: "Ayetler",
                column: "SureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ayetler_Sureler_SureId",
                table: "Ayetler",
                column: "SureId",
                principalTable: "Sureler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ayetler_Temalar_TemaId",
                table: "Ayetler",
                column: "TemaId",
                principalTable: "Temalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ayetler_Sureler_SureId",
                table: "Ayetler");

            migrationBuilder.DropForeignKey(
                name: "FK_Ayetler_Temalar_TemaId",
                table: "Ayetler");

            migrationBuilder.DropTable(
                name: "Sureler");

            migrationBuilder.DropIndex(
                name: "IX_Ayetler_SureId",
                table: "Ayetler");

            migrationBuilder.DropColumn(
                name: "SureId",
                table: "Ayetler");

            migrationBuilder.AlterColumn<int>(
                name: "TemaId",
                table: "Ayetler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Ayetler",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ayetler_Temalar_TemaId",
                table: "Ayetler",
                column: "TemaId",
                principalTable: "Temalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
