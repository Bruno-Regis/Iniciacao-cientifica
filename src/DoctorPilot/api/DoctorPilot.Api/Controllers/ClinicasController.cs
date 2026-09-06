using DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas;
using DoctorPilot.Contracts.Clinicas;
using DoctorPilot.Core.Enums;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/clinicas")]
    [ApiController]
    public class ClinicasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClinicasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/clinicas
        [HttpPost]
        [Authorize(Roles = nameof(RoleEnum.SuperAdmin))]
        public async Task<IActionResult> Post([FromBody] CreateClinicaRequest request)
        {
            var command = new InsertClinicasCommand
            {
                Nome = request.Nome,
                Email = request.Email,
                Cnpj = request.Cnpj,
                AdminEmail = request.Administrador?.Email ?? string.Empty,
                AdminSenha = request.Administrador?.Senha ?? string.Empty
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(Post), new { id = result.Data }, result);
        }
    }
}
