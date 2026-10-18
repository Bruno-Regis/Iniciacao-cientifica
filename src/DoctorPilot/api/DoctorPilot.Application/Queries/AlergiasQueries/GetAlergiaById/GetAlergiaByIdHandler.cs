using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById
{
    public class GetAlergiaByIdHandler : IQueryHandler<GetAlergiaByIdQuery, ResultViewModel<AlergiaDto>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAlergiaByIdHandler(IRepository<Alergia> alergiaRepository, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _alergiaRepository = alergiaRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<AlergiaDto>> Handle(GetAlergiaByIdQuery query, CancellationToken cancellationToken)
        {
            var alergia = await _alergiaRepository.GetByIdAsync(query.Id);

            if (alergia is null)
                throw new NotFoundException("Alergia não encontrada.");

            var paciente = await _pacienteRepository.GetByIdAsync(alergia.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<AlergiaDto>.Error("Alergia não encontrada.");

            return ResultViewModel<AlergiaDto>.Success(alergia.ToDto());
        }
    }
}
