using DoctorPilot.Application.Services;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.HorariosProfissionaisCommands.InsertHorarioProfissional
{
    
    public class InsertHorarioProfissionalHandler : ICommandHandler<InsertHorarioProfissionalCommand, ResultViewModel<Guid>>
    {
        private readonly IHorarioProfissionalRepository _horarioRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public InsertHorarioProfissionalHandler(IHorarioProfissionalRepository horarioRepository, IProfissionalRepository profissionalRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _horarioRepository = horarioRepository;
            _profissionalRepository = profissionalRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(InsertHorarioProfissionalCommand command, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUserService.GetRequiredClinicaId();

            if (command.HoraFim <= command.HoraInicio)
                return ResultViewModel<Guid>.Error("O horário final deve ser posterior ao horário inicial.");

            var profissional = await _profissionalRepository.GetByIdAsync(command.ProfissionalId);

            if (profissional is null)
                throw new NotFoundException("Profissional não encontrado.");

            if (profissional.ClinicaId != clinicaId)
                return ResultViewModel<Guid>.Error("Profissional não encontrado.");

            var existeConflito = await _horarioRepository.ExistsConflitoAsync(command.ProfissionalId,command.DiaSemana,command.HoraInicio,command.HoraFim,cancellationToken);

            if (existeConflito)
                throw new ConflictException("Já existe um horário cadastrado para este profissional neste dia e horário.");

            var horario = new HorarioProfissional(command.ProfissionalId, command.DiaSemana, command.HoraInicio, command.HoraFim);

            await _horarioRepository.AddAsync(horario);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultViewModel<Guid>.Success(horario.Id);
        }

    }
}
