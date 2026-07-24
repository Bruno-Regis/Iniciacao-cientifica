using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente
{
    public class GetAllAgendamentosByPacienteHandler : IQueryHandler<GetAllAgendamentosByPacienteQuery, ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAllAgendamentosByPacienteHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemDto>>> Handle(GetAllAgendamentosByPacienteQuery query, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = agendamentos.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Success(viewModels);
        }
    }
}
