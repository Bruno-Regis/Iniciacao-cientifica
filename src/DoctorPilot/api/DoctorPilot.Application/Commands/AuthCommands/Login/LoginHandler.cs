using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Auth;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AuthCommands.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand, ResultViewModel<LoginResponseDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LoginHandler(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<LoginResponseDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(command.Email);

            if (usuario is null)
                return ResultViewModel<LoginResponseDto>.Error("Email ou senha inválidos.");

            if (!usuario.IsActive)
                return ResultViewModel<LoginResponseDto>.Error("Usuário inativo.");

            if (!_passwordHasher.Verify(command.Senha, usuario.Senha))
                return ResultViewModel<LoginResponseDto>.Error("Email ou senha inválidos.");

            var (token, expiresAt) = _tokenGenerator.GenerateToken(usuario);

            usuario.AtualizarUltimoAcesso();
            await _usuarioRepository.UpdateAsync(usuario);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<LoginResponseDto>.Success(new LoginResponseDto
            {
                Token = token,
                UsuarioId = usuario.Id,
                ClinicaId = usuario.ClinicaId,
                Email = usuario.Email,
                Role = usuario.Role.ToString(),
                ExpiresAt = expiresAt
            });
        }
    }
}
