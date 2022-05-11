using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerManagementAPI.Migrations
{
    public partial class DeletedEmailAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workers_Login",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Workers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Workers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Login",
                table: "Workers",
                column: "Login",
                unique: true,
                filter: "[Login] IS NOT NULL");
        }
    }
}
