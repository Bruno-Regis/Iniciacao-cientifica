using DoctorPilot.Application.DTOs;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
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
            template.Sessoes = Sessoes.Select(s =>
            {
                var sessao = new AnamneseTemplateSessao(
                    template.Id,
                    s.Pergunta.Trim(),
                    Enum.Parse<AnamneseSessaoTypeEnum>(s.Tipo, true),
                    s.Ordem,
                    s.Obrigatoria);
                sessao.Opcoes = s.Opcoes
                    .Select(o => new AnamneseTemplateOpcao(sessao.Id, o.Texto.Trim(), o.Ordem))
                    .ToList();
                return sessao;
            }).ToList();
            return template;
        }
    }
}
