using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranGuide.Infrastructure.Factories
{
    public class KuranGuideDbContextFactory : IDesignTimeDbContextFactory<KuranGuideDbContext>
    {
        public KuranGuideDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KuranGuideDbContext>();

            // Migration sırasında appsettings okunamadığı için burada connection string MANUEL verilir
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-B1KU30T\\SQLEXPRESS2019;Initial Catalog=KuranGuideDb;User Id=sa;Password=768399;TrustServerCertificate=true;");

            return new KuranGuideDbContext(optionsBuilder.Options);
        }
    }
}
