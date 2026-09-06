using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class UsuariosConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.Senha)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();
            builder
                .Property(u => u.ClinicaId)
                .IsRequired(false);

            builder 
                .HasOne(u => u.Clinica)
                    .WithMany()
                    .HasForeignKey(u => u.ClinicaId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
