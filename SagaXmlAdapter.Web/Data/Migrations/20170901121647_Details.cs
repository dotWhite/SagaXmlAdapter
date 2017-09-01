using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class Details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_FileDetail_FileDetailId",
                table: "InvoiceDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_Client_ClientId",
                table: "InvoiceHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_Provider_ProviderId",
                table: "InvoiceHeader");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "InvoiceHeader",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "InvoiceHeader",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "InvoiceHeader",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "InvoiceDetail",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_FileDetail_FileDetailId",
                table: "InvoiceDetail",
                column: "FileDetailId",
                principalTable: "FileDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_Client_ClientId",
                table: "InvoiceHeader",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader",
                column: "FileDetailId",
                principalTable: "FileDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_Provider_ProviderId",
                table: "InvoiceHeader",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_FileDetail_FileDetailId",
                table: "InvoiceDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_Client_ClientId",
                table: "InvoiceHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeader_Provider_ProviderId",
                table: "InvoiceHeader");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "InvoiceHeader",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "InvoiceHeader",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "InvoiceHeader",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "InvoiceDetail",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_FileDetail_FileDetailId",
                table: "InvoiceDetail",
                column: "FileDetailId",
                principalTable: "FileDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_Client_ClientId",
                table: "InvoiceHeader",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_FileDetail_FileDetailId",
                table: "InvoiceHeader",
                column: "FileDetailId",
                principalTable: "FileDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeader_Provider_ProviderId",
                table: "InvoiceHeader",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
