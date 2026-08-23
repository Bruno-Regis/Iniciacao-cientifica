using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios
{
    public class GetAllUsuariosHandler : IQueryHandler<GetAllUsuariosQuery, ResultViewModel<IEnumerable<UsuarioItemDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllUsuariosHandler(IUsuarioRepository usuarioRepository, ICurrentUserService currentUser)
        {
            _usuarioRepository = usuarioRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<UsuarioItemDto>>> Handle(GetAllUsuariosQuery query, CancellationToken cancellationToken)
        {
            var usuarios = _currentUser.IsSuperAdmin
                ? await _usuarioRepository.GetAllAsync(query.Page, query.PageSize)
                : await _usuarioRepository.GetByClinicaIdAsync(_currentUser.ClinicaId, query.Page, query.PageSize);
            var viewModels = usuarios.Select(u => u.ToItemDto());
            return ResultViewModel<IEnumerable<UsuarioItemDto>>.Success(viewModels);
        }
    }
}

