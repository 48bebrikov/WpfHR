using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfHR.Migrations
{
    /// <inheritdoc />
    public partial class AddApproversAndDevelopersStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DevelopersString",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproversString",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevelopersString",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ApproversString",
                table: "Modules");
        }

    }
}
