using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using Mediator;

namespace DoctorPilot.Application.Commands.UsuariosCommands.InsertUsuarios
{
    public class InsertUsuariosCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid ClinicaId { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public RoleEnum Role { get; set; }

        public Usuario ToEntity(string senhaHash)
            => new(ClinicaId, Email, senhaHash, Role);
    }
}
