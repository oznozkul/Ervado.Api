using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ervado.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class firms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "DomainId",
                table: "ApplicationUsers",
                newName: "FirmId");

            migrationBuilder.CreateTable(
                name: "Firms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ParentFirm = table.Column<int>(type: "integer", nullable: true),
                    FirmId = table.Column<int>(type: "integer", nullable: false),
                    CreatedUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<int>(type: "integer", nullable: true),
                    DeleteDate = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_FirmId",
                table: "ApplicationUsers",
                column: "FirmId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Firms_FirmId",
                table: "ApplicationUsers",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Firms_FirmId",
                table: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_FirmId",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "FirmId",
                table: "ApplicationUsers",
                newName: "DomainId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: true);
        }
    }
}
