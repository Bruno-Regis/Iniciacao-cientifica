using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamnesesConfiguration : IEntityTypeConfiguration<Anamnese>
    {
        public void Configure(EntityTypeBuilder<Anamnese> builder)
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
                .Property(a => a.NomeDaAnamnese)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(a => a.Observacoes)
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
                .HasOne(a => a.AnamneseTemplate)
                .WithMany()
                .HasForeignKey(a => a.AnamneseTemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
