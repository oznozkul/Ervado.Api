using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ervado.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ApplicationUsers",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ApplicationUsers",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteDate",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteUserId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DeleteUserId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "ApplicationUsers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "ApplicationUsers",
                newName: "CreatedAt");
        }
    }
}
