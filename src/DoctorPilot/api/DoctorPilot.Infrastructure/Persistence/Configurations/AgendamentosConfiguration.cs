using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AgendamentosConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
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

            builder
                .Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder
                .Property(a => a.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder
                .Property(a => a.Procedimento)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(a => a.DataHoraInicial)
                .IsRequired();

            builder
                .Property(a => a.DataHoraFinal)
                .IsRequired();

            builder
                .Property(a => a.Sala)
                .IsRequired(false)
                .HasMaxLength(50);

            builder
                .Property(a => a.Confirmacao)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(a => a.Descricao)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder
                .HasOne<Paciente>()
                .WithMany()
                .HasForeignKey(a => a.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<Profissional>()
                .WithMany()
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<Clinica>()
                .WithMany()
                .HasForeignKey(a => a.ClinicaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(a => new { a.ClinicaId, a.DataHoraInicial }); // Para otimizar consultas por clínica e data/hora inicial
        }
    }
}
