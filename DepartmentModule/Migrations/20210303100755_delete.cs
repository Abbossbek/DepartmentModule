using Microsoft.EntityFrameworkCore.Migrations;

namespace DepartmentModule.Migrations
{
    public partial class delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Book_ProgramId",
                table: "Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Book_ThemesId",
                table: "Subject");

            migrationBuilder.DropTable(
                name: "IBooks");

            migrationBuilder.RenameColumn(
                name: "ThemesId",
                table: "Subject",
                newName: "ThemesID");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Subject",
                newName: "ProgramID");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "Subject",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subject",
                newName: "SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_ThemesId",
                table: "Subject",
                newName: "IX_Subject_ThemesID");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_ProgramId",
                table: "Subject",
                newName: "IX_Subject_ProgramID");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "Book",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Book",
                newName: "BookID");

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Subject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SubjectAdditionalLiterature",
                columns: table => new
                {
                    SubjectAdditionalLiteratureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    AdditionalLiteratureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAdditionalLiterature", x => x.SubjectAdditionalLiteratureID);
                    table.ForeignKey(
                        name: "FK_SubjectAdditionalLiterature_Book_AdditionalLiteratureId",
                        column: x => x.AdditionalLiteratureId,
                        principalTable: "Book",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAdditionalLiterature_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectLiterature",
                columns: table => new
                {
                    SubjectLiteratureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    LiteratureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLiterature", x => x.SubjectLiteratureID);
                    table.ForeignKey(
                        name: "FK_SubjectLiterature_Book_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "Book",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectLiterature_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAdditionalLiterature_AdditionalLiteratureId",
                table: "SubjectAdditionalLiterature",
                column: "AdditionalLiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAdditionalLiterature_SubjectId",
                table: "SubjectAdditionalLiterature",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLiterature_LiteratureId",
                table: "SubjectLiterature",
                column: "LiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLiterature_SubjectId",
                table: "SubjectLiterature",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ProgramID",
                table: "Subject",
                column: "ProgramID",
                principalTable: "Book",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ThemesID",
                table: "Subject",
                column: "ThemesID",
                principalTable: "Book",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Book_ProgramID",
                table: "Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Book_ThemesID",
                table: "Subject");

            migrationBuilder.DropTable(
                name: "SubjectAdditionalLiterature");

            migrationBuilder.DropTable(
                name: "SubjectLiterature");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Subject");

            migrationBuilder.RenameColumn(
                name: "ThemesID",
                table: "Subject",
                newName: "ThemesId");

            migrationBuilder.RenameColumn(
                name: "ProgramID",
                table: "Subject",
                newName: "ProgramId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Subject",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "Subject",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_ThemesID",
                table: "Subject",
                newName: "IX_Subject_ThemesId");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_ProgramID",
                table: "Subject",
                newName: "IX_Subject_ProgramId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Book",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "BookID",
                table: "Book",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "IBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IBooks_Book_Id",
                        column: x => x.Id,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IBooks_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IBooks_SubjectId",
                table: "IBooks",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ProgramId",
                table: "Subject",
                column: "ProgramId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Book_ThemesId",
                table: "Subject",
                column: "ThemesId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
