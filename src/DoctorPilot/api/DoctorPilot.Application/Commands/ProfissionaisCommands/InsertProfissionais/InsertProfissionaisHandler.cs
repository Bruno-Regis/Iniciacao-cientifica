
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais
{
    public class InsertProfissionaisHandler : ICommandHandler<InsertProfissionaisCommand, ResultViewModel<Guid>>
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IUsuarioRepository _usuarioRepository;

        public InsertProfissionaisHandler(IProfissionalRepository profissionalRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, IUsuarioRepository usuarioRepository)
        {
            _profissionalRepository = profissionalRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _usuarioRepository = usuarioRepository;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertProfissionaisCommand command, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(command.UsuarioId);
            if (usuario is null || usuario.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<Guid>.Error("Usuário não encontrado.");

            var usuarioJaEhProfissional = await _profissionalRepository.GetByUsuarioIdAsync(command.UsuarioId);
            if (usuarioJaEhProfissional is not null)
                return ResultViewModel<Guid>.Error("Este usuário já está cadastrado como profissional.");

            var profissional = command.ToEntity(_currentUser.ClinicaId); // Cria a entidade Profissional a partir do comando e do ClinicaId do usuário atual

            await _profissionalRepository.AddAsync(profissional);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(profissional.Id);
        }
    }
}
