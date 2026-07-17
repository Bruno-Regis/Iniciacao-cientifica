using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios
{
    public class GetAllUsuariosHandler : IQueryHandler<GetAllUsuariosQuery, ResultViewModel<IEnumerable<UsuarioItemViewModel>>>
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public GetAllUsuariosHandler(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<UsuarioItemViewModel>>> Handle(GetAllUsuariosQuery query, CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioRepository.GetAllAsync(query.Page, query.PageSize);
            var viewModels = usuarios.Select(UsuarioItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<UsuarioItemViewModel>>.Success(viewModels);
        }
    }
}

