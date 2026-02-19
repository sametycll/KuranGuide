using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class TemaConfiguration : IEntityTypeConfiguration<Tema>
    {
        public void Configure(EntityTypeBuilder<Tema> builder)
        {
            builder.ToTable("Temalar");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TemaAdi)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Aciklama)
                .HasMaxLength(int.MaxValue);

            builder.Property(t => t.AnahtarKelimeler)
                .HasMaxLength(int.MaxValue);

            builder.Property(t => t.Icon)
       .HasMaxLength(100)
       .IsRequired(false);

        }
    }
}
