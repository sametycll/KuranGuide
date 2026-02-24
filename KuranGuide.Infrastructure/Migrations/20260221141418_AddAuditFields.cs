using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Temalar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Temalar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Temalar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sureler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sureler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sureler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hadisler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Hadisler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Hadisler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Favoriler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Favoriler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ayetler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Ayetler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ayetler",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Temalar");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Temalar");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Temalar");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sureler");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sureler");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Sureler");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hadisler");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Hadisler");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Hadisler");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Favoriler");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Favoriler");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ayetler");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Ayetler");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ayetler");
        }
    }
}
