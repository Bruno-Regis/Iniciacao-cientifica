using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById
{
    public class GetUsuarioByIdHandler : IQueryHandler<GetUsuarioByIdQuery, ResultViewModel<UsuarioDetailsDto>>
    {
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly ICurrentUserService _currentUser;

        public GetUsuarioByIdHandler(IRepository<Usuario> usuarioRepository, ICurrentUserService currentUser)
        {
            _usuarioRepository = usuarioRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<UsuarioDetailsDto>> Handle(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(query.Id);

            if (usuario is null || (!_currentUser.IsSuperAdmin && usuario.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<UsuarioDetailsDto>.Error("Usuário não encontrado.");

            return ResultViewModel<UsuarioDetailsDto>.Success(usuario.ToDetailsDto());
        }
    }
}
