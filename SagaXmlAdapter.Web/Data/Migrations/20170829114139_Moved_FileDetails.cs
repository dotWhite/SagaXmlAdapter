using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class Moved_FileDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileDetailId",
                table: "InvoiceHeader",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeader_FileDetailId",
                table: "InvoiceHeader",
                column: "FileDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader",
                column: "FileDetailId",
                principalTable: "FileDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceHeader_FileDetailId",
                table: "InvoiceHeader");

            migrationBuilder.DropColumn(
                name: "FileDetailId",
                table: "InvoiceHeader");
        }
    }
}
