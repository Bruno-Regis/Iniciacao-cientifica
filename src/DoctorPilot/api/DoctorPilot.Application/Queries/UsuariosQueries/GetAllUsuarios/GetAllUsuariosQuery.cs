using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Usuarios;
using Mediator;

namespace DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios
{
    public class GetAllUsuariosQuery : IQuery<ResultViewModel<IEnumerable<UsuarioItemViewModel>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
