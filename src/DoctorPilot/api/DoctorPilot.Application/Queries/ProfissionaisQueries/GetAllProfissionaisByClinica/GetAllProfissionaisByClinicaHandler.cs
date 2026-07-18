using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Profissionais;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalByClinica;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica
{
    public class GetAllProfissionaisByClinicaHandler : IQueryHandler<GetAllProfissionaisByClinicaQuery, ResultViewModel<IEnumerable<ProfissionalItemViewModel>>>
    {
        private readonly IProfissionalRepository _profissionalRepository;

        public GetAllProfissionaisByClinicaHandler(IProfissionalRepository profissionalRepository)
        {
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<ProfissionalItemViewModel>>> Handle(GetAllProfissionaisByClinicaQuery query, CancellationToken cancellationToken)
        {
            var profissionais = await _profissionalRepository.GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);
            var viewModels = profissionais.Select(ProfissionalItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<ProfissionalItemViewModel>>.Success(viewModels);
        }
    }
}
