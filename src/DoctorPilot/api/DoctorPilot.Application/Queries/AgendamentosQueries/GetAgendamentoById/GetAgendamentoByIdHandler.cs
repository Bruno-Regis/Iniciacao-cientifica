using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById
{
    public class GetAgendamentoByIdHandler : IQueryHandler<GetAgendamentoByIdQuery, ResultViewModel<AgendamentoDetailsDto>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAgendamentoByIdHandler(IAgendamentoRepository agendamentoRepository, ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<AgendamentoDetailsDto>> Handle(GetAgendamentoByIdQuery query, CancellationToken cancellationToken)
        {
            var resultado = await _agendamentoRepository.GetByIdComNomesAsync(query.Id,cancellationToken);

            if (resultado is null)
                throw new NotFoundException("Agendamento não encontrado.");

            var agendamento = resultado.Agendamento;

            if (!_currentUser.IsSuperAdmin &&
                agendamento.ClinicaId != _currentUser.ClinicaId)
            {
                return ResultViewModel<AgendamentoDetailsDto>
                    .Error("Agendamento não encontrado.");
            }

            return ResultViewModel<AgendamentoDetailsDto>
                .Success(resultado.ToDetailsDto());
        }
    }
}
