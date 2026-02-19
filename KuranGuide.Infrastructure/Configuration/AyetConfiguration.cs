using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KuranGuide.Infrastructure.Configuration
{
    public class AyetConfiguration : IEntityTypeConfiguration<Ayet>
    {
        public void Configure(EntityTypeBuilder<Ayet> builder)
        {
            builder.ToTable("Ayetler");

            builder.HasKey(a => a.Id);



            builder.Property(a => a.AyetNo)
                .IsRequired();

            builder.Property(a => a.ArapcaMetin)
                .IsRequired();

            builder.Property(a => a.Meal)
                .IsRequired();

            builder.Property(a => a.Aciklama)
                .HasMaxLength(int.MaxValue);

            // YENİ İLİŞKİ: Sure
            builder.HasOne(a => a.Sure)
                   .WithMany(s => s.Ayetler)
                   .HasForeignKey(a => a.SureId);

            // GÜNCELLEME: Tema (Artık Opsiyonel Olabilir)
            builder.HasOne(a => a.Tema)
                   .WithMany(t => t.Ayetler)
                   .HasForeignKey(a => a.TemaId)
                   .IsRequired(false) // <-- Nullable yaptık
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
