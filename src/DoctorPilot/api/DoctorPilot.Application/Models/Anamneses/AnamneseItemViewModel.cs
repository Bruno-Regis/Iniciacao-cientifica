using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Models.Anamneses
{
    public class AnamneseItemViewModel
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public DateTime CreatedAt { get; set; }

        public static AnamneseItemViewModel FromEntity(Anamnese anamnese)
            => new()
            {
                Id = anamnese.Id,
                PacienteId = anamnese.PacienteId,
                ProfissionalId = anamnese.ProfissionalId,
                NomeDaAnamnese = anamnese.NomeDaAnamnese,
                CreatedAt = anamnese.CreatedAt
            };
    }
}
