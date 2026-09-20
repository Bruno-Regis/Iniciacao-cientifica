using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.HorariosProfissionais;
using Mediator;

namespace DoctorPilot.Application.Queries.HorariosProfissionaisQueries.GetHorariosByProfissional
{
    public class GetHorariosByProfissionalQuery : IQuery<ResultViewModel<IEnumerable<HorarioProfissionalDto>>>
    {
        public GetHorariosByProfissionalQuery(Guid profissionalId)
        {
            ProfissionalId = profissionalId;
        }

        public Guid ProfissionalId { get; }
    }
}
