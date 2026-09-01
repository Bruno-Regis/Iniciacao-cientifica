using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentosElegiveisParaEvolucaoByPaciente
{
    public class GetAgendamentosElegiveisParaEvolucaoByPacienteQuery
        : IQuery<ResultViewModel<IEnumerable<AgendamentoElegivelEvolucaoDto>>>
    {
        public GetAgendamentosElegiveisParaEvolucaoByPacienteQuery(Guid pacienteId)
        {
            PacienteId = pacienteId;
        }

        public Guid PacienteId { get; }
    }
}
