using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class ChatsConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
