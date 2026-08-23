using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class EvolucoesConfiguration : IEntityTypeConfiguration<Evolucao>
    {
        public void Configure(EntityTypeBuilder<Evolucao> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(e => e.Conteudo)
                .IsRequired()
                .HasMaxLength(4000);

            builder
                .HasIndex(e => e.AgendamentoId)
                .IsUnique();

            builder
                .HasOne<Agendamento>()
                .WithMany()
                .HasForeignKey(e => e.AgendamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
