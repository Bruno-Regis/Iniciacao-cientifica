using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.InsertAgendamentos
{
    public class InsertAgendamentoHandler : ICommandHandler<InsertAgendamentoCommand, ResultViewModel<Guid>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAgendamentoHandler(IAgendamentoRepository agendamentoRepository, IUnitOfWork unitOfWork)
        {
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAgendamentoCommand command, CancellationToken cancellationToken)
        {
            var conflitos = await _agendamentoRepository.GetByPeriodoAsync(command.ClinicaId, command.DataHoraInicial, command.DataHoraFinal, command.ProfissionalId);

            var conflitoComProfissional = conflitos.Any(a => a.ProfissionalId == command.ProfissionalId);
            if (conflitoComProfissional)
                return ResultViewModel<Guid>.Error("Profissional já possui um agendamento neste período.");

            var conflitoComSala = command.Sala is not null && conflitos.Any(a => a.Sala == command.Sala);
            if (conflitoComSala)
                return ResultViewModel<Guid>.Error("Sala já está ocupada neste período.");

            var agendamento = command.ToEntity();   
            await _agendamentoRepository.AddAsync(agendamento);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(agendamento.Id);
        }
    }
}

