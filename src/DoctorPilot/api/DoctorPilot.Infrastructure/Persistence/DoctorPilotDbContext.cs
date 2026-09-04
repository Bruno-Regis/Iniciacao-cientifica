using DoctorPilot.Core.Entities;
using DoctorPilot.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Persistence
{
    public class DoctorPilotDbContext : DbContext
    {
        public DoctorPilotDbContext(DbContextOptions<DoctorPilotDbContext> options)
            : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Alergia> Alergias { get; set; }
        public DbSet<Anamnese> Anamneses { get; set; }
        public DbSet<AnamneseSessao> AnamneseSessoes { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Evolucao> Evolucoes { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Transcricao> Transcricoes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMensagem> ChatMensagens { get; set; }
        public DbSet<AnamneseTemplate> AnamneseTemplates { get; set; }
        public DbSet<AnamneseTemplateSessao> AnamneseTemplateSessoes { get; set; }
        public DbSet<AnamneseTemplateOpcao> AnamneseTemplateOpcoes { get; set; }
        public DbSet<AnamneseSessaoOpcao> AnamneseSessaoOpcoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsuariosConfiguration());
            builder.ApplyConfiguration(new AgendamentosConfiguration());
            builder.ApplyConfiguration(new AlergiasConfiguration());
            builder.ApplyConfiguration(new AnamnesesConfiguration());
            builder.ApplyConfiguration(new AnamneseSessoesConfiguration());
            builder.ApplyConfiguration(new ClinicasConfiguration());
            builder.ApplyConfiguration(new EvolucoesConfiguration());
            builder.ApplyConfiguration(new PacientesConfiguration());
            builder.ApplyConfiguration(new ProfissionaisConfiguration());
            builder.ApplyConfiguration(new TranscricoesConfiguration());
            builder.ApplyConfiguration(new ChatsConfiguration());
            builder.ApplyConfiguration(new ChatMensagensConfiguration());
            builder.ApplyConfiguration(new AnamneseTemplateConfiguration());
            builder.ApplyConfiguration(new AnamneseTemplateSessaoConfiguration());
            builder.ApplyConfiguration(new AnamneseTemplateOpcoesConfiguration());
            builder.ApplyConfiguration(new AnamneseSessaoOpcoesConfiguration());

            foreach (var entityType in builder.Model.GetEntityTypes()
                         .Where(entityType => typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "entity");
                var isDeleted = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Not(isDeleted),
                    parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
