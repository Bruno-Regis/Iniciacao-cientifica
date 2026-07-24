using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById
{
    public class GetAgendamentoByIdHandler : IQueryHandler<GetAgendamentoByIdQuery, ResultViewModel<AgendamentoDetailsDto>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAgendamentoByIdHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<AgendamentoDetailsDto>> Handle(GetAgendamentoByIdQuery query, CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(query.Id);

            if (agendamento is null)
                return ResultViewModel<AgendamentoDetailsDto>.Error("Agendamento não encontrado.");

            return ResultViewModel<AgendamentoDetailsDto>.Success(agendamento.ToDetailsDto());
        }
    }
}
