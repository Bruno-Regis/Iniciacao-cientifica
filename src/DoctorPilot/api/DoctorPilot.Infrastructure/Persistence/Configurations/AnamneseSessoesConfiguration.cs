using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamneseSessoesConfiguration : IEntityTypeConfiguration<AnamneseSessao>
    {
        public void Configure(EntityTypeBuilder<AnamneseSessao> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(s => s.Pergunta)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .Property(s => s.Resposta)
                .IsRequired()
                .HasMaxLength(2000);

            builder
                .Property(s => s.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder
                .Property(s => s.Ordem)
                .IsRequired();

            // Nav prop existe em Anamnese (Sessoes), então HasOne com lambda
            builder
                .HasOne(s => s.Anamnese)
                .WithMany(a => a.Sessoes)
                .HasForeignKey(s => s.AnamneseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
