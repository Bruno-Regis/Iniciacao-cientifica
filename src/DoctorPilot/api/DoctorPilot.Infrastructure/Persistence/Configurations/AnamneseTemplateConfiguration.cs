using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamneseTemplateConfiguration : IEntityTypeConfiguration<AnamneseTemplate>
    {
        public void Configure(EntityTypeBuilder<AnamneseTemplate> builder)
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
                .Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(a => a.Descricao)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder
                .HasOne(a => a.Clinica)
                .WithMany()
                .HasForeignKey(a => a.ClinicaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
