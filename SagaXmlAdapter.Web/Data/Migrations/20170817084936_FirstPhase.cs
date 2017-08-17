using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Http;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    public partial class FirstPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenant_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(nullable: true),
                    CIF = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(nullable: true),
                    CIF = table.Column<string>(nullable: true),
                    Capital = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provider_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: true),
                    ClientSoldInfo = table.Column<string>(nullable: true),
                    Currecy = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    InversTaxing = table.Column<bool>(nullable: false),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Observations = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<IFormFile>(nullable: true),
                    ProviderId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false),
                    TotalVat = table.Column<decimal>(nullable: false),
                    VAT = table.Column<decimal>(nullable: false),
                    VatCollecting = table.Column<bool>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    CsvFile = table.Column<IFormFile>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceHeader_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceHeader_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceHeader_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    BarCode = table.Column<string>(nullable: true),
                    CodeClient = table.Column<string>(nullable: true),
                    CodeProvider = table.Column<string>(nullable: true),
                    InvoiceHeaderId = table.Column<int>(nullable: true),
                    MeasurementUnit = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    VAT = table.Column<decimal>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    VatPercentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetail_InvoiceHeader_InvoiceHeaderId",
                        column: x => x.InvoiceHeaderId,
                        principalTable: "InvoiceHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_TenantId",
                table: "Client",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_InvoiceHeaderId",
                table: "InvoiceDetail",
                column: "InvoiceHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeader_ClientId",
                table: "InvoiceHeader",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeader_ProviderId",
                table: "InvoiceHeader",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeader_TenantId",
                table: "InvoiceHeader",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_TenantId",
                table: "Provider",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_UserId",
                table: "Tenant",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "InvoiceHeader");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
