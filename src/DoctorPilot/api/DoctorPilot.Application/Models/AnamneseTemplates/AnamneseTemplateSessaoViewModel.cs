using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Models.AnamneseTemplates
{
    public class AnamneseTemplateSessaoViewModel
    {
        public Guid Id { get; set; }
        public string Pergunta { get; set; }
        public string Tipo { get; set; }

        public static AnamneseTemplateSessaoViewModel FromEntity(AnamneseTemplateSessao sessao)
            => new()
            {
                Id = sessao.Id,
                Pergunta = sessao.Pergunta,
                Tipo = sessao.Tipo.ToString()
            };
    }
}
