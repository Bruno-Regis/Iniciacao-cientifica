using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucoesByPaciente
{
    public class GetEvolucoesByPacienteHandler : IQueryHandler<GetEvolucoesByPacienteQuery, ResultViewModel<IEnumerable<EvolucaoDto>>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;

        public GetEvolucoesByPacienteHandler(IEvolucaoRepository evolucaoRepository)
        {
            _evolucaoRepository = evolucaoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<EvolucaoDto>>> Handle(GetEvolucoesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var evolucoes = await _evolucaoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = evolucoes.Select(e => e.ToDto());
            return ResultViewModel<IEnumerable<EvolucaoDto>>.Success(viewModels);
        }
    }
}
