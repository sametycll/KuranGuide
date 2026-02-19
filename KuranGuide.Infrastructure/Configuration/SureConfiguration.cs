using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class SureConfiguration :IEntityTypeConfiguration<Sure>
    {
        public void Configure(EntityTypeBuilder<Sure> builder)
        {
            builder.ToTable("Sureler");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SureAdi).IsRequired().HasMaxLength(100);
            builder.Property(s => s.ArapcaAdi).HasMaxLength(100);

            // İlişki Tanımı: Bir Surenin çok Ayeti olur
            builder.HasMany(s => s.Ayetler)
                   .WithOne(a => a.Sure)
                   .HasForeignKey(a => a.SureId)
                   .OnDelete(DeleteBehavior.Cascade); // Sure silinirse ayetleri de silinsin
        }
    }
}
