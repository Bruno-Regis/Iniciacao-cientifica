using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class TranscricoesConfiguration : IEntityTypeConfiguration<Transcricao>
    {
        public void Configure(EntityTypeBuilder<Transcricao> builder)
        {
            builder
                .HasKey(t => t.Id);

            builder
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(t => t.TextoTranscrito)
                .IsRequired();
            // sem HasMaxLength — texto gerado por IA, tamanho imprevisível

            builder
                .HasOne<Agendamento>()
                .WithMany()
                .HasForeignKey(t => t.AgendamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
