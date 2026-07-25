using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Usuarios;
using Mediator;


namespace DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById
{
    public class GetUsuarioByIdQuery :IQuery<ResultViewModel<UsuarioDetailsDto>>
    {
        public GetUsuarioByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
