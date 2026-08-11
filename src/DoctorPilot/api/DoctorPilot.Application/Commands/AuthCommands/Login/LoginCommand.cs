using DoctorPilot.Contracts.Auth;
using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.AuthCommands.Login
{
    public class LoginCommand : ICommand<ResultViewModel<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
