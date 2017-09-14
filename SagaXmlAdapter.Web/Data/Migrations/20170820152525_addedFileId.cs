using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class addedFileId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileDetailId",
                table: "InvoiceDetail",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<byte[]>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_FileDetailId",
                table: "InvoiceDetail",
                column: "FileDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_FileDetails_FileDetailId",
                table: "InvoiceDetail",
                column: "FileDetailId",
                principalTable: "FileDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_FileDetails_FileDetailId",
                table: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "FileDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetail_FileDetailId",
                table: "InvoiceDetail");

            migrationBuilder.DropColumn(
                name: "FileDetailId",
                table: "InvoiceDetail");
        }
    }
}
