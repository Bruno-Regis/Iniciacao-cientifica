using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamneseSessaoOpcoesConfiguration : IEntityTypeConfiguration<AnamneseSessaoOpcao>
    {
        public void Configure(EntityTypeBuilder<AnamneseSessaoOpcao> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(o => o.Texto)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.Ordem)
                .IsRequired();

            builder.Property(o => o.Selecionada)
                .IsRequired();

            builder.HasOne(o => o.AnamneseSessao)
                .WithMany(s => s.Opcoes)
                .HasForeignKey(o => o.AnamneseSessaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => new { o.AnamneseSessaoId, o.Ordem });
        }
    }
}
