using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas
{
    public class InsertClinicasCommand : ICommand<ResultViewModel<Guid>> 
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminSenha { get; set; } = string.Empty;

        public Clinica ToEntity()
            => new(Nome, Email, Cnpj);
    }
}
