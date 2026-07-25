
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais
{
    public class InsertProfissionaisHandler : ICommandHandler<InsertProfissionaisCommand, ResultViewModel<Guid>>
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertProfissionaisHandler(IProfissionalRepository profissionalRepository, IUnitOfWork unitOfWork)
        {
            _profissionalRepository = profissionalRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertProfissionaisCommand command, CancellationToken cancellationToken)
        {
            var usuarioJaEhProfissional = await _profissionalRepository.GetByUsuarioIdAsync(command.UsuarioId);
            if (usuarioJaEhProfissional is not null)
                return ResultViewModel<Guid>.Error("Este usuário já está cadastrado como profissional.");

            var profissional = command.ToEntity();

            await _profissionalRepository.AddAsync(profissional);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(profissional.Id);
        }
    }
}
