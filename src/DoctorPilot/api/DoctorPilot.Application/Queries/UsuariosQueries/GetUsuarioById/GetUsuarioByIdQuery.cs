using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Usuarios;
using Mediator;


namespace DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById
{
    public class GetUsuarioByIdQuery :IQuery<ResultViewModel<UsuarioDetailsViewModel>>
    {
        public GetUsuarioByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
