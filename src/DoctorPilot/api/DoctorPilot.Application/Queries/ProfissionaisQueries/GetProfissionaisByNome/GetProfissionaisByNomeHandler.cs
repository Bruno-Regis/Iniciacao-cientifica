using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionaisByNome
{
    public class GetProfissionaisByNomeHandler : IQueryHandler<GetProfissionaisByNomeQuery, ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetProfissionaisByNomeHandler(IProfissionalRepository profissionalRepository,ICurrentUserService currentUserService)
        {
            _profissionalRepository = profissionalRepository;
            _currentUserService = currentUserService;
        }

        public async ValueTask<ResultViewModel<IEnumerable<ProfissionalItemDto>>> Handle(GetProfissionaisByNomeQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUserService.GetRequiredClinicaId();
            
            var profissionais = await _profissionalRepository.SearchByNomeAsync(
                clinicaId,
                query.Nome,
                cancellationToken: cancellationToken);

            var profissionaisDto = profissionais.Select(p => p.ToItemDto());

            return ResultViewModel<IEnumerable<ProfissionalItemDto>>.Success(profissionaisDto);
        }
    }
}
