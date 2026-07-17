using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById
{
    public class GetUsuarioByIdHandler : IQueryHandler<GetUsuarioByIdQuery, ResultViewModel<UsuarioDetailsViewModel>>
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public GetUsuarioByIdHandler(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async ValueTask<ResultViewModel<UsuarioDetailsViewModel>> Handle(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(query.Id);

            if (usuario is null)
                return ResultViewModel<UsuarioDetailsViewModel>.Error("Usuário não encontrado.");

            return ResultViewModel<UsuarioDetailsViewModel>.Success(UsuarioDetailsViewModel.FromEntity(usuario));
        }
    }
}
