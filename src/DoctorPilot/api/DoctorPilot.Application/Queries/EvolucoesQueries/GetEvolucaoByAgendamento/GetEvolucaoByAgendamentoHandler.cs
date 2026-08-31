using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento
{
    public class GetEvolucaoByAgendamentoHandler : IQueryHandler<GetEvolucaoByAgendamentoQuery, ResultViewModel<EvolucaoDto>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICurrentUserService _currentUser;

        public GetEvolucaoByAgendamentoHandler(IEvolucaoRepository evolucaoRepository, IAgendamentoRepository agendamentoRepository, ICurrentUserService currentUser)
        {
            _evolucaoRepository = evolucaoRepository;
            _agendamentoRepository = agendamentoRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<EvolucaoDto>> Handle(GetEvolucaoByAgendamentoQuery query, CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(query.AgendamentoId);
            if (agendamento is null || (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<EvolucaoDto>.Error("Agendamento não encontrado.");

            var evolucao = await _evolucaoRepository.GetByAgendamentoIdAsync(
                query.AgendamentoId,
                cancellationToken);

            if (evolucao is null)
                return ResultViewModel<EvolucaoDto>.Error("Evolução não encontrada para este agendamento.");

            return ResultViewModel<EvolucaoDto>.Success(evolucao.ToDto());
        }
    }
}
