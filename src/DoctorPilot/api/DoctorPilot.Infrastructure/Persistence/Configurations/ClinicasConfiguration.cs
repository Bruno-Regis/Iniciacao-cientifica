using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class ClinicasConfiguration : IEntityTypeConfiguration<Clinica>
    {
        public void Configure(EntityTypeBuilder<Clinica> builder)
        {
            builder 
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasIndex(c => c.Email)
                .IsUnique();

            builder
                .Property(c => c.Cnpj)
                .HasMaxLength(14)
                .IsRequired();

            builder 
                .HasIndex(c => c.Cnpj)
                .IsUnique();

            builder
                .Property(a => a.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
        }
    }
}
