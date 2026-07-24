using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAllAnamnesesByPaciente
{
    public class GetAllAnamnesesByPacienteHandler : IQueryHandler<GetAllAnamnesesByPacienteQuery, ResultViewModel<IEnumerable<AnamneseItemDto>>>
    {

        private readonly IAnamneseRepository _anamneseRepository;

        public GetAllAnamnesesByPacienteHandler(IAnamneseRepository anamneseRepository)
        {
            _anamneseRepository = anamneseRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AnamneseItemDto>>> Handle(GetAllAnamnesesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var anamneses = await _anamneseRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize);
            var viewModels = anamneses.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AnamneseItemDto>>.Success(viewModels);
        }
    }
}
