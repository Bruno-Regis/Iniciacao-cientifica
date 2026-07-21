using DoctorPilot.Application.DTOs;
using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseCommands
{
    public class InsertAnamneseCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public string? Observacoes { get; set; }
        public List<InsertAnamneseSessaoDto> Sessoes { get; set; } = [];

        public Anamnese ToEntity()
        {
            var anamnese = new Anamnese(PacienteId, ProfissionalId, NomeDaAnamnese, Observacoes);
            anamnese.Sessoes = Sessoes
                .Select(s => new AnamneseSessao(anamnese.Id, s.Pergunta, s.Resposta, s.Tipo))
                .ToList();
            return anamnese;
        }
    }
}
