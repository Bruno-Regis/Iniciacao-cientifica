using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseFromTemplate
{
    public class InsertAnamneseFromTemplateCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid PacienteId { get; set; }
        public Guid TemplateId { get; set; }
        public string? Observacoes { get; set; }
        public List<InsertAnamneseFromTemplateRespostaDto> Respostas { get; set; } = [];
    }

    public class InsertAnamneseFromTemplateRespostaDto
    {
        public Guid PerguntaId { get; set; }
        public string? Resposta { get; set; }
        public List<Guid> OpcaoIds { get; set; } = [];
    }
}
