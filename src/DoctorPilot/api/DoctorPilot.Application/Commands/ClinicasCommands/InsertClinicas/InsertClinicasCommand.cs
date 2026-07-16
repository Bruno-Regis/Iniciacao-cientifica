using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas
{
    public class InsertClinicasCommand : ICommand<ResultViewModel<Guid>> 
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }

        public Clinica ToEntity()
            => new(Nome, Email, Cnpj);
    }
}
