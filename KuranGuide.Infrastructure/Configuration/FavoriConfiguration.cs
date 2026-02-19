using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class FavoriConfiguration : IEntityTypeConfiguration<Favori>
    {
        public void Configure(EntityTypeBuilder<Favori> builder)
        {
            builder.ToTable("Favoriler");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Type)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(f => f.RefId)
                .IsRequired();

            builder.Property(f => f.CreatedAt)
                .IsRequired();

            builder.HasOne(f => f.Kullanici)
                .WithMany(k => k.Favoriler)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
