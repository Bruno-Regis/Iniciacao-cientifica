using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.HorariosProfissionais;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.HorariosProfissionaisQueries.GetHorariosByProfissional
{
    public class GetHorariosByProfissionalHandler : IQueryHandler<GetHorariosByProfissionalQuery, ResultViewModel<IEnumerable<HorarioProfissionalDto>>>
    {
        private readonly IHorarioProfissionalRepository _horarioRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUserService;
        public GetHorariosByProfissionalHandler(IHorarioProfissionalRepository horarioRepository, IProfissionalRepository profissionalRepository, ICurrentUserService currentUserService)
        {
            _horarioRepository = horarioRepository;
            _profissionalRepository = profissionalRepository;
            _currentUserService = currentUserService;
        }
        public async ValueTask<ResultViewModel<IEnumerable<HorarioProfissionalDto>>> Handle(GetHorariosByProfissionalQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUserService.GetRequiredClinicaId();

            var profissional = await _profissionalRepository
                .GetByIdAsync(query.ProfissionalId);

            if (profissional is null)
                throw new NotFoundException("Profissional não encontrado.");

            if (profissional.ClinicaId != clinicaId)
                return ResultViewModel<IEnumerable<HorarioProfissionalDto>>
                    .Error("Profissional não encontrado.");

            var horarios = await _horarioRepository
                .GetByProfissionalIdAsync(
                    query.ProfissionalId,
                    cancellationToken);

            var horariosDto = horarios
                .Select(h => h.ToDto());

            return ResultViewModel<IEnumerable<HorarioProfissionalDto>>
                .Success(horariosDto);
        }
    }
}
