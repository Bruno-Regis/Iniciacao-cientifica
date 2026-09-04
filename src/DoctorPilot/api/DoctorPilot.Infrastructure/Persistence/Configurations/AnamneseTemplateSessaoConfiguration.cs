using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamneseTemplateSessaoConfiguration : IEntityTypeConfiguration<AnamneseTemplateSessao>
    {
        public void Configure(EntityTypeBuilder<AnamneseTemplateSessao> builder)
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
                .Property(s => s.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder
                .Property(s => s.Ordem)
                .IsRequired();

            builder
                .Property(s => s.Obrigatoria)
                .IsRequired();

            builder
                .HasOne<AnamneseTemplate>()
                .WithMany(a => a.Sessoes)
                .HasForeignKey(s => s.AnamneseTemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
