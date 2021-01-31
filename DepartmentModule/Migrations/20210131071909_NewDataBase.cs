using Microsoft.EntityFrameworkCore.Migrations;

namespace DepartmentModule.Migrations
{
    public partial class NewDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProgramId = table.Column<int>(nullable: false),
                    ThemesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    SubjectId = table.Column<int>(nullable: true),
                    SubjectId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Book_Subject_SubjectId1",
                        column: x => x.SubjectId1,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_SubjectId",
                table: "Book",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_SubjectId1",
                table: "Book",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ProgramId",
                table: "Subject",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ThemesId",
                table: "Subject",
                column: "ThemesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ProgramId",
                table: "Subject",
                column: "ProgramId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ThemesId",
                table: "Subject",
                column: "ThemesId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Subject_SubjectId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Subject_SubjectId1",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
