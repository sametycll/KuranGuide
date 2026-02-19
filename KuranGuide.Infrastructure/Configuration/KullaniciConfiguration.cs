using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class KullaniciConfiguration : IEntityTypeConfiguration<Kullanici>
    {
        public void Configure(EntityTypeBuilder<Kullanici> builder)
        {
            builder.ToTable("Kullanicilar");

            builder.HasKey(k => k.Id);

            builder.Property(k => k.Ad)
                .IsRequired().HasDefaultValue("")
                .HasMaxLength(200);

            builder.Property(k => k.Soyad)
                .IsRequired().HasDefaultValue("")
                .HasMaxLength(200);

            builder.HasIndex(k => k.Email)
                .IsUnique();

            builder.Property(k => k.PasswordHash)
                .IsRequired();

            builder.Property(k => k.Role)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(k => k.CreatedAt)
                .IsRequired();
        }
    }
}
