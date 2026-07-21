using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAllAnamnesesByPaciente
{
    public class GetAllAnamnesesByPacienteHandler : IQueryHandler<GetAllAnamnesesByPacienteQuery, ResultViewModel<IEnumerable<AnamneseItemViewModel>>>
    {

        private readonly IAnamneseRepository _anamneseRepository;

        public GetAllAnamnesesByPacienteHandler(IAnamneseRepository anamneseRepository)
        {
            _anamneseRepository = anamneseRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AnamneseItemViewModel>>> Handle(GetAllAnamnesesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var anamneses = await _anamneseRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = anamneses.Select(AnamneseItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<AnamneseItemViewModel>>.Success(viewModels);
        }
    }
}
