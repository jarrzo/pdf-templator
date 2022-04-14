using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared;

namespace pdfTemplator.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<PdfTemplate> PdfTemplates { get; set; } = null!;
        public DbSet<PdfConversion> PdfConversions { get; set; } = null!;
    }
}