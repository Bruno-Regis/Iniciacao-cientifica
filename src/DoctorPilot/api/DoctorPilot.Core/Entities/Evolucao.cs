using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Entities
{
    public class Evolucao : BaseEntity
    {
        public Evolucao(Guid agendamentoId, string conteudo)
        {
            AgendamentoId = agendamentoId;
            Conteudo = conteudo;
        }

        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; }
    }
}
