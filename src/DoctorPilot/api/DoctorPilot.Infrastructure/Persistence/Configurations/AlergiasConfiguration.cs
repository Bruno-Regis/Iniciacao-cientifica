using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AlergiasConfiguration : IEntityTypeConfiguration<Alergia>
    {
        public void Configure(EntityTypeBuilder<Alergia> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(a => a.PacienteId)
                .IsRequired();

            builder.Property(a => a.NomeAlergia)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Gravidade)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(a => a.DataIdentificacao)
                .IsRequired(false);

            builder.Property(a => a.Observacoes)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder
                .HasOne(a => a.Paciente)
                .WithMany(p => p.Alergias)
                .HasForeignKey(a => a.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
