using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class ShowInvoiceDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "showInvoiceDetails",
                table: "InvoiceHeader",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "showInvoiceDetails",
                table: "InvoiceHeader");
        }
    }
}
