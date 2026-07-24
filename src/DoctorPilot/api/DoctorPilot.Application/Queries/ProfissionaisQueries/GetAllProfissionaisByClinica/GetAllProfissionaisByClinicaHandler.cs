using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalByClinica;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica
{
    public class GetAllProfissionaisByClinicaHandler : IQueryHandler<GetAllProfissionaisByClinicaQuery, ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        private readonly IProfissionalRepository _profissionalRepository;

        public GetAllProfissionaisByClinicaHandler(IProfissionalRepository profissionalRepository)
        {
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<ProfissionalItemDto>>> Handle(GetAllProfissionaisByClinicaQuery query, CancellationToken cancellationToken)
        {
            var profissionais = await _profissionalRepository.GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);
            var viewModels = profissionais.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<ProfissionalItemDto>>.Success(viewModels);
        }
    }
}
