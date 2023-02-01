using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserApplication.Migrations
{
    /// <inheritdoc />
    public partial class Authentication1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "authentication",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "authentication",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "authentication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "authentication",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "authentication");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "authentication");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "authentication",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "authentication",
                newName: "password");
        }
    }
}
