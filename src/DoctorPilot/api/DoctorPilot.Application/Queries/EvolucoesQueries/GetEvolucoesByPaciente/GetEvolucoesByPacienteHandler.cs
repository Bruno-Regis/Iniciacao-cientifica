using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucoesByPaciente
{
    public class GetEvolucoesByPacienteHandler : IQueryHandler<GetEvolucoesByPacienteQuery, ResultViewModel<IEnumerable<EvolucaoDto>>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetEvolucoesByPacienteHandler(IEvolucaoRepository evolucaoRepository, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _evolucaoRepository = evolucaoRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<IEnumerable<EvolucaoDto>>> Handle(GetEvolucoesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(query.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<IEnumerable<EvolucaoDto>>.Error("Paciente não encontrado.");

            var evolucoes = await _evolucaoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = evolucoes.Select(e => e.ToDto());
            return ResultViewModel<IEnumerable<EvolucaoDto>>.Success(viewModels);
        }
    }
}
