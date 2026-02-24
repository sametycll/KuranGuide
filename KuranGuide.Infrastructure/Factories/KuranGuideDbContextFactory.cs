using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KuranGuide.Infrastructure.Factories
{
    public class KuranGuideDbContextFactory : IDesignTimeDbContextFactory<KuranGuideDbContext>
    {
        public KuranGuideDbContext CreateDbContext(string[] args)
        {
            // Migration sırasında appsettings.Development.json'dan connection string okunur
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../KuranGuide.Api"))
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<KuranGuideDbContext>();
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly("KuranGuide.Infrastructure"));

            return new KuranGuideDbContext(optionsBuilder.Options);
        }
    }
}
