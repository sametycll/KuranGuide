using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Context
{
    public class KuranGuideDbContext : DbContext
    {
        public KuranGuideDbContext(DbContextOptions<KuranGuideDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Tema> Temalar { get; set; }
        public DbSet<Ayet> Ayetler { get; set; }
        public DbSet<Hadis> Hadisler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Favori> Favoriler { get; set; }
        public DbSet<Sure> Sureler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(KuranGuideDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
