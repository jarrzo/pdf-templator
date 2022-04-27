using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pdfTemplator.Server.Migrations
{
    public partial class AddPdfInsertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfInsertable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PdfTemplateId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfInsertable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfInsertable_PdfTemplates_PdfTemplateId",
                        column: x => x.PdfTemplateId,
                        principalTable: "PdfTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfInsertable_PdfTemplateId",
                table: "PdfInsertable",
                column: "PdfTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfInsertable");
        }
    }
}
