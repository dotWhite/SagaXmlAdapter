using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SagaXmlAdapter.Web.Data;

namespace SagaXmlAdapter.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170822082458_AddedFileItems")]
    partial class AddedFileItems
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Bank");

                    b.Property<string>("CIF");

                    b.Property<string>("Description");

                    b.Property<string>("IBAN");

                    b.Property<string>("Name");

                    b.Property<string>("RegNumber");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.FileDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("FileType");

                    b.Property<int>("Length");

                    b.HasKey("Id");

                    b.ToTable("FileDetail");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.InvoiceDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo");

                    b.Property<string>("BarCode");

                    b.Property<string>("CodeClient");

                    b.Property<string>("CodeProvider");

                    b.Property<int?>("FileDetailId");

                    b.Property<int?>("InvoiceHeaderId");

                    b.Property<string>("MeasurementUnit");

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.Property<string>("Observations");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("Quantity");

                    b.Property<decimal>("VAT");

                    b.Property<decimal>("Value");

                    b.Property<decimal>("VatPercentage");

                    b.HasKey("Id");

                    b.HasIndex("FileDetailId");

                    b.HasIndex("InvoiceHeaderId");

                    b.ToTable("InvoiceDetail");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.InvoiceHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientId");

                    b.Property<string>("ClientSoldInfo");

                    b.Property<string>("Currecy");

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<bool>("InversTaxing");

                    b.Property<DateTime>("IssueDate");

                    b.Property<string>("Number");

                    b.Property<string>("Observations");

                    b.Property<string>("PaymentMethod");

                    b.Property<int?>("ProviderId");

                    b.Property<int?>("TenantId");

                    b.Property<decimal>("TotalAmount");

                    b.Property<decimal>("TotalValue");

                    b.Property<decimal>("TotalVat");

                    b.Property<decimal>("VAT");

                    b.Property<bool>("VatCollecting");

                    b.Property<decimal>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProviderId");

                    b.HasIndex("TenantId");

                    b.ToTable("InvoiceHeader");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Bank");

                    b.Property<string>("CIF");

                    b.Property<decimal>("Capital");

                    b.Property<string>("Description");

                    b.Property<string>("IBAN");

                    b.Property<string>("Name");

                    b.Property<string>("RegNumber");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tenant");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SagaXmlAdapter.Web.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Client", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.InvoiceDetail", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.FileDetail", "FileDetail")
                        .WithMany("Content")
                        .HasForeignKey("FileDetailId");

                    b.HasOne("SagaXmlAdapter.Web.Models.InvoiceHeader")
                        .WithMany("Details")
                        .HasForeignKey("InvoiceHeaderId");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.InvoiceHeader", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("SagaXmlAdapter.Web.Models.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId");

                    b.HasOne("SagaXmlAdapter.Web.Models.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Provider", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("SagaXmlAdapter.Web.Models.Tenant", b =>
                {
                    b.HasOne("SagaXmlAdapter.Web.Models.ApplicationUser", "User")
                        .WithMany("Tenants")
                        .HasForeignKey("UserId");
                });
        }
    }
}
