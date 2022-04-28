using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Models;

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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
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
        public DbSet<PdfInsertable> PdfInsertables { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PdfTemplate>().HasData(
                new PdfTemplate
                {
                    Id = 1,
                    Name = "Test1",
                    Description = "Description1",
                    Content = "Content1",
                },
                new PdfTemplate
                {
                    Id = 2,
                    Name = "Test2",
                    Description = "Description2",
                    Content = "Content2",
                },
                new PdfTemplate
                {
                    Id = 3,
                    Name = "Test3",
                    Description = "Description3",
                    Content = "Content3",
                }
            );
            builder.Entity<PdfConversion>().HasData(
                new PdfConversion { Id = 1, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 2, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 3, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 4, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 5, PdfTemplateId = 2, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 6, PdfTemplateId = 2, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(0) },
                new PdfConversion { Id = 7, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-1) },
                new PdfConversion { Id = 8, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-1) },
                new PdfConversion { Id = 9, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 10, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 11, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 12, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 13, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 14, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 15, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-2) },
                new PdfConversion { Id = 16, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-3) },
                new PdfConversion { Id = 17, PdfTemplateId = 1, DataJSON = "", PdfPath = "", CreatedAt = DateTime.Now.AddDays(-3) }
            );
            base.OnModelCreating(builder);
        }
    }
}