using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente
{
    public class GetAllAgendamentosByPacienteHandler : IQueryHandler<GetAllAgendamentosByPacienteQuery, ResultViewModel<IEnumerable<AgendamentoItemViewModel>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAllAgendamentosByPacienteHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemViewModel>>> Handle(GetAllAgendamentosByPacienteQuery query, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = agendamentos.Select(AgendamentoItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<AgendamentoItemViewModel>>.Success(viewModels);
        }
    }
}
