using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class ChatMensagensConfiguration : IEntityTypeConfiguration<ChatMensagem>
    {
        public void Configure(EntityTypeBuilder<ChatMensagem> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(m => m.Conteudo)
                .IsRequired();
            // sem HasMaxLength — conteúdo de IA pode ser longo

            builder
                .Property(m => m.TipoMensagem)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder
                .HasOne(m => m.Chat)
                .WithMany(c => c.Mensagens)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
