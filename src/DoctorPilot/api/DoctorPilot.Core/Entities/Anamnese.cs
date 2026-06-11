using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Entities
{
    public class Anamnese : BaseEntity
    {
        public Anamnese(Guid pacienteId, Guid profissionalId, string nomeDaAnamnese, string? observacoes)
        {
            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            NomeDaAnamnese = nomeDaAnamnese;
            Observacoes = observacoes;
        }

        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public string? Observacoes { get; set; }
    }
}
