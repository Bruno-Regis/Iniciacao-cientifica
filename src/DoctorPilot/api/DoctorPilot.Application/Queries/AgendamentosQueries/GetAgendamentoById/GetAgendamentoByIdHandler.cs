using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById
{
    public class GetAgendamentoByIdHandler : IQueryHandler<GetAgendamentoByIdQuery, ResultViewModel<AgendamentoDetailsViewModel>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAgendamentoByIdHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<AgendamentoDetailsViewModel>> Handle(GetAgendamentoByIdQuery query, CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(query.Id);

            if (agendamento is null)
                return ResultViewModel<AgendamentoDetailsViewModel>.Error("Agendamento não encontrado.");

            return ResultViewModel<AgendamentoDetailsViewModel>.Success(AgendamentoDetailsViewModel.FromEntity(agendamento));
        }
    }
}
