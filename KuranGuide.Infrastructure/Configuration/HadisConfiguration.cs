using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class HadisConfiguration : IEntityTypeConfiguration<Hadis>
    {
        public void Configure(EntityTypeBuilder<Hadis> builder)
        {
            builder.ToTable("Hadisler");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Kaynak)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.Metin)
                .IsRequired();

            builder.Property(h => h.Aciklama)
                .HasMaxLength(int.MaxValue);

            builder.HasOne(h => h.Tema)
                .WithMany(t => t.Hadisler)
                .HasForeignKey(h => h.TemaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
