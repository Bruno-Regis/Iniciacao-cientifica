using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
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
        private readonly IPasswordHasher _passwordHasher;

        public InsertProfissionaisHandler(
            IProfissionalRepository profissionalRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher)
        {
            _profissionalRepository = profissionalRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(
            InsertProfissionaisCommand command,
            CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.GetRequiredClinicaId();

            if (await _usuarioRepository.ExistsEmailAsync(command.Email))
                return ResultViewModel<Guid>.Error("Já existe um usuário com este e-mail.");

            var senhaHash = _passwordHasher.Hash(command.Senha);
            var usuario = new Usuario(clinicaId, command.Email, senhaHash, RoleEnum.Medico);
            var profissional = command.ToEntity(clinicaId, usuario.Id);

            await _usuarioRepository.AddAsync(usuario);
            await _profissionalRepository.AddAsync(profissional);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultViewModel<Guid>.Success(profissional.Id);
        }
    }
}
