using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById
{
    public class GetUsuarioByIdHandler : IQueryHandler<GetUsuarioByIdQuery, ResultViewModel<UsuarioDetailsDto>>
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public GetUsuarioByIdHandler(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async ValueTask<ResultViewModel<UsuarioDetailsDto>> Handle(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(query.Id);

            if (usuario is null)
                return ResultViewModel<UsuarioDetailsDto>.Error("Usuário não encontrado.");

            return ResultViewModel<UsuarioDetailsDto>.Success(usuario.ToDetailsDto());
        }
    }
}
