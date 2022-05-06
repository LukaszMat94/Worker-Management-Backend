using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerManagementAPI.Migrations
{
    public partial class AddTechnologyProjectMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Projects_ProjectId",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "IX_Technologies_ProjectId",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Technologies");

            migrationBuilder.CreateTable(
                name: "ProjectsTechnologies",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    TechnologyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsTechnologies", x => new { x.ProjectId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_ProjectsTechnologies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsTechnologies_TechnologyId",
                table: "ProjectsTechnologies",
                column: "TechnologyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectsTechnologies");

            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "Technologies",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_ProjectId",
                table: "Technologies",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Projects_ProjectId",
                table: "Technologies",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
