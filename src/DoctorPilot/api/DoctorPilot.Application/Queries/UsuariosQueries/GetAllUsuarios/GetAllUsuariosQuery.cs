using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using Mediator;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios
{
    public class GetAllUsuariosQuery : IQuery<ResultViewModel<IEnumerable<UsuarioItemDto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
