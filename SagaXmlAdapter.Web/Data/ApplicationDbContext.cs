using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SagaXmlAdapter.Web.Models;

namespace SagaXmlAdapter.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<SagaXmlAdapter.Web.Models.Client> Client { get; set; }

        public DbSet<SagaXmlAdapter.Web.Models.Provider> Provider { get; set; }

        public DbSet<SagaXmlAdapter.Web.Models.Tenant> Tenant { get; set; }

        public DbSet<SagaXmlAdapter.Web.Models.InvoiceHeader> InvoiceHeader { get; set; }

        public DbSet<SagaXmlAdapter.Web.Models.InvoiceDetail> InvoiceDetail { get; set; }
    }
}
