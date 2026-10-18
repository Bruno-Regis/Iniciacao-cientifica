using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.UpdateAgendamento
{
    public class UpdateAgendamentoHandler : ICommandHandler<UpdateAgendamentoCommand, ResultViewModel<Guid>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateAgendamentoHandler(IAgendamentoRepository agendamentoRepository, IPacienteRepository pacienteRepository, IProfissionalRepository profissionalRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(UpdateAgendamentoCommand command, CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id);
            if (agendamento is null)
                throw new NotFoundException("Agendamento não encontrado.");

            if (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<Guid>.Error("Agendamento não encontrado.");

            if (agendamento.Status == StatusAgendamentoEnum.cancelado)
                return ResultViewModel<Guid>.Error("Não é possível editar um agendamento cancelado.");

            if (command.DataHoraFinal <= command.DataHoraInicial)
                return ResultViewModel<Guid>.Error("A data/hora final deve ser posterior à data/hora inicial.");

            var clinicaId = agendamento.ClinicaId;
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null)
                throw new NotFoundException("Paciente não encontrado.");

            if (paciente.ClinicaId != clinicaId)
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var profissional = await _profissionalRepository.GetByIdAsync(command.ProfissionalId);
            if (profissional is null)
                throw new NotFoundException("Profissional não encontrado.");

            if (profissional.ClinicaId != clinicaId)
                return ResultViewModel<Guid>.Error("Profissional não encontrado.");

            if (await _agendamentoRepository.ExisteConflitoProfissionalAsync(clinicaId, command.ProfissionalId, command.DataHoraInicial, command.DataHoraFinal, command.Id, cancellationToken))
                return ResultViewModel<Guid>.Error("Profissional já possui um agendamento neste período.");

            if (!string.IsNullOrWhiteSpace(command.Sala) &&
                await _agendamentoRepository.ExisteConflitoSalaAsync(clinicaId, command.Sala, command.DataHoraInicial, command.DataHoraFinal, command.Id, cancellationToken))
                return ResultViewModel<Guid>.Error("Sala já está ocupada neste período.");

            agendamento.Atualizar(command.PacienteId, command.ProfissionalId, command.Tipo, command.Procedimento, command.DataHoraInicial, command.DataHoraFinal, command.Sala, command.Descricao);
            await _agendamentoRepository.UpdateAsync(agendamento);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(agendamento.Id);
        }
    }
}
