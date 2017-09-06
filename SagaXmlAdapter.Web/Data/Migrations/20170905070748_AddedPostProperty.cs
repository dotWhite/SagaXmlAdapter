using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class AddedPostProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "showInvoiceDetails",
                table: "InvoiceHeader",
                newName: "isPost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPost",
                table: "InvoiceHeader",
                newName: "showInvoiceDetails");
        }
    }
}
