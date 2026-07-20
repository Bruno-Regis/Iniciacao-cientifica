using DoctorPilot.Application.DTOs;
using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Commands.AnamneseTemplatesCommands.InsertAnamneseTemplate
{
    public class InsertAnamneseTemplateCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<InsertAnamneseTemplateSessaoDto> Sessoes { get; set; } = [];

        public AnamneseTemplate ToEntity()
        {
            var template = new AnamneseTemplate(ClinicaId, Nome, Descricao);
            template.Sessoes = Sessoes
                .Select(s => new AnamneseTemplateSessao(template.Id, s.Pergunta, s.Tipo))
                .ToList();
            return template;
        }
    }
}
