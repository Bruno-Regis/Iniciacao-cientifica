using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucoesByPaciente
{
    public class GetEvolucoesByPacienteHandler : IQueryHandler<GetEvolucoesByPacienteQuery, ResultViewModel<IEnumerable<EvolucaoViewModel>>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;

        public GetEvolucoesByPacienteHandler(IEvolucaoRepository evolucaoRepository)
        {
            _evolucaoRepository = evolucaoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<EvolucaoViewModel>>> Handle(GetEvolucoesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var evolucoes = await _evolucaoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = evolucoes.Select(EvolucaoViewModel.FromEntity);
            return ResultViewModel<IEnumerable<EvolucaoViewModel>>.Success(viewModels);
        }
    }
}
