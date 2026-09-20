using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class ProfissionaisConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            builder
                .Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();
            builder
                .Property(p => p.Profissao)
                .HasMaxLength (100)
                .IsRequired();
            builder
                .Property(p => p.FotoUrl);

            builder
                .Property(p => p.Crm)
                .HasMaxLength(100);

            builder
                .HasOne(p => p.Clinica)
                    .WithMany()
                    .HasForeignKey(p => p.ClinicaId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Usuario)
                    .WithOne()
                    .HasForeignKey<Profissional>(p => p.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder
                .Navigation(p => p.Horarios)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
