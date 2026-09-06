using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.UsuariosCommands.InsertUsuarios
{
    public class InsertUsuariosHandler : ICommandHandler<InsertUsuariosCommand, ResultViewModel<Guid>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUser;

        public InsertUsuariosHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ICurrentUserService currentUser)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertUsuariosCommand command, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(command.Role))
                return ResultViewModel<Guid>.Error("A role informada e invalida.");

            if (!_currentUser.IsSuperAdmin &&
                command.Role is not RoleEnum.Medico and not RoleEnum.Admnistrativo)
            {
                return ResultViewModel<Guid>.Error(
                    "A role informada nao pode ser atribuida por um administrador de clinica.");
            }

            var usuarioJaExiste = await _usuarioRepository.ExistsEmailAsync(command.Email);

            if (usuarioJaExiste)
                return ResultViewModel<Guid>.Error("Já existe um usuário com este e-mail.");
            
            var senhaHash = _passwordHasher.Hash(command.Senha);

            var usuario = command.ToEntity(_currentUser.GetRequiredClinicaId(), senhaHash);

            await _usuarioRepository.AddAsync(usuario);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(usuario.Id);
        }
    }
}
