using DoctorPilot.Application.DTOs;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate
{
    public class InsertAnamneseTemplateCommand : ICommand<ResultViewModel<Guid>>
    {
      //  public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<InsertAnamneseTemplateSessaoDto> Sessoes { get; set; } = [];

        public AnamneseTemplate ToEntity(Guid clinicaId)
        {
            var template = new AnamneseTemplate(clinicaId, Nome, Descricao);
            template.Sessoes = Sessoes
                .Select(s => new AnamneseTemplateSessao(template.Id, s.Pergunta, s.Tipo))
                .ToList();
            return template;
        }
    }
}
