using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Pacientes
{
    public class PacienteItemViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Status { get; set; }

        public static PacienteItemViewModel FromEntity(Paciente paciente)
            => new()
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Email = paciente.Email,
                Celular = paciente.Celular,
                Status = paciente.Status.ToString()
            };
    }
}
