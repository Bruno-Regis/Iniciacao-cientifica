using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class AnamneseTemplateOpcoesConfiguration : IEntityTypeConfiguration<AnamneseTemplateOpcao>
    {
        public void Configure(EntityTypeBuilder<AnamneseTemplateOpcao> builder)
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

            builder.HasOne(o => o.AnamneseTemplateSessao)
                .WithMany(s => s.Opcoes)
                .HasForeignKey(o => o.AnamneseTemplateSessaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => new { o.AnamneseTemplateSessaoId, o.Ordem });
        }
    }
}
