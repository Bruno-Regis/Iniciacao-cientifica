using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios
{
    public class GetAllUsuariosHandler : IQueryHandler<GetAllUsuariosQuery, ResultViewModel<IEnumerable<UsuarioItemDto>>>
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public GetAllUsuariosHandler(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<UsuarioItemDto>>> Handle(GetAllUsuariosQuery query, CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioRepository.GetAllAsync(query.Page, query.PageSize);
            var viewModels = usuarios.Select(u => u.ToItemDto());
            return ResultViewModel<IEnumerable<UsuarioItemDto>>.Success(viewModels);
        }
    }
}

