using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
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

        public DbSet<Template> Templates { get; set; } = null!;
        public DbSet<Conversion> Conversions { get; set; } = null!;
        public DbSet<Field> Fields { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<DataSource> DataSources { get; set; } = null!;
        public DbSet<AutomatedTemplate> AutomatedTemplates { get; set; } = null!;
    }
}