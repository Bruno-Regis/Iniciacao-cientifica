using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseManual
{
    public class InsertAnamneseManualCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid PacienteId { get; set; }
        public string NomeDaAnamnese { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public List<InsertAnamneseManualSessaoDto> Sessoes { get; set; } = [];
    }

    public class InsertAnamneseManualSessaoDto
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public string? Resposta { get; set; }
        public List<InsertAnamneseManualOpcaoDto> Opcoes { get; set; } = [];
    }

    public class InsertAnamneseManualOpcaoDto
    {
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public bool Selecionada { get; set; }
    }
}
