using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pdfTemplator.Server.Models;

namespace pdfTemplator.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (
                    x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                ((BaseModel)entity.Entity).UpdatedAt = DateTime.Now;

                if (entity.State == EntityState.Added)
                    ((BaseModel)entity.Entity).CreatedAt = DateTime.Now;
            }
        }

        public DbSet<PdfTemplate> PdfTemplates { get; set; } = null!;
        public DbSet<PdfConversion> PdfConversions { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
    }
}