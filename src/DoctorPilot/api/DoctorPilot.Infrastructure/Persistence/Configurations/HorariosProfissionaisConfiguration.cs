using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class HorariosProfissionaisConfiguration : IEntityTypeConfiguration<HorarioProfissional>
    {
        public void Configure(EntityTypeBuilder<HorarioProfissional> builder)
        {
            builder
                .HasKey(h => h.Id);

            builder
                .Property(h => h.DiaSemana)
                .IsRequired();

            builder
                .Property(h => h.HoraInicio)
                .IsRequired();

            builder
                .Property(h => h.HoraFim)
                .IsRequired();

            builder
                .HasOne(h => h.Profissional)
                    .WithMany(p => p.Horarios)
                    .HasForeignKey(h => h.ProfissionalId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(h => new
                {
                    h.ProfissionalId,
                    h.DiaSemana
                });
        }
    }
}
