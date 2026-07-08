using DoctorPilot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorPilot.Infrastructure.Persistence.Configurations
{
    public class PacientesConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder
                .Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.Nome)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.Cpf)
                .HasMaxLength (11)
                .IsRequired();
            builder
                .HasIndex(p => p.Cpf)
                .IsUnique();

            builder
                .Property(p => p.Celular)
                .HasMaxLength (13)
                .IsRequired();

            builder
                .Property(p => p.Telefone)
                .HasMaxLength(13);

            builder
                .Property(p => p.Rg)
                .HasMaxLength(20);

            builder 
                .Property(p => p.OrgaoExpedidor)
                .HasMaxLength(20);

            builder
                .Property(p => p.CEP)
                .HasMaxLength(8)
                .IsRequired();

            builder
               .Property(p => p.Cidade)
               .HasMaxLength(100)
               .IsRequired();

            builder
               .Property(p => p.Estado)
               .HasMaxLength(2)
               .IsRequired();

            builder
               .Property(p => p.Bairro)
               .HasMaxLength(100)
               .IsRequired();

            builder
               .Property(p => p.Numero)
               .HasMaxLength(10)
               .IsRequired();

            builder 
                .Property(p => p.Complemento)
                .HasMaxLength(100);

            builder
               .Property(p => p.Profissao)
               .HasMaxLength(100)
               .IsRequired();

            builder
                .Property(p => p.Observacoes)
                .HasMaxLength(1000);

            builder
                .Property(p => p.DataNascimento)
                .IsRequired();

            builder
                .Property(p => p.Genero)
                .HasConversion<string>()
                .IsRequired();

            builder
                .Property(p => p.EstadoCivil)
                .HasConversion<string>()
                .IsRequired();

            builder
                .Property(p => p.NecessidadeEspecial)
                .HasConversion<string>()
                .IsRequired();

            builder
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.NomeDaMae)
                .HasMaxLength(200);

            builder
                .Property(p => p.NomeDoPai)
                .HasMaxLength(200);

            builder
            .HasOne(p => p.Clinica)
                .WithMany()
                .HasForeignKey(p => p.ClinicaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
