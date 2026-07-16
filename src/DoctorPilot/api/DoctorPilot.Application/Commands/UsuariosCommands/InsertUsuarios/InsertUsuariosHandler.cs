using DoctorPilot.Application.Models;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.UsuariosCommands.InsertUsuarios
{
    public class InsertUsuariosHandler : ICommandHandler<InsertUsuariosCommand, ResultViewModel<Guid>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertUsuariosHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertUsuariosCommand command, CancellationToken cancellationToken)
        {
            var usuarioJaExiste = await _usuarioRepository.ExistsEmailAsync(command.Email);

            if (usuarioJaExiste)
                return ResultViewModel<Guid>.Error("Já existe um usuário com este e-mail.");
            
            var usuario = command.ToEntity();

            await _usuarioRepository.AddAsync(usuario);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(usuario.Id);
        }
    }
}
