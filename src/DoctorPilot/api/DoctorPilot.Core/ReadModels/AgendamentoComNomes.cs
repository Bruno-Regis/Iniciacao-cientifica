using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.ReadModels
{
    public class AgendamentoComNomes
    {
        public AgendamentoComNomes(Agendamento agendamento, string pacienteNome, string profissionalNome)
        {
            Agendamento = agendamento;
            PacienteNome = pacienteNome;
            ProfissionalNome = profissionalNome;
        }

        public Agendamento Agendamento { get; }
        public string PacienteNome { get; }
        public string ProfissionalNome { get; }
    }
}
