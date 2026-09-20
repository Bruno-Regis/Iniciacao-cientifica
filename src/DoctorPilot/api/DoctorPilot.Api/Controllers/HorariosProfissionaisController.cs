using DoctorPilot.Application.Commands.HorariosProfissionaisCommands.InsertHorarioProfissional;
using DoctorPilot.Application.Queries.HorariosProfissionaisQueries.GetHorariosByProfissional;
using DoctorPilot.Contracts.HorariosProfissionais;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [ApiController]
    [Route("api/horarios-profissionais")]
    [Authorize(Roles = "TenantAdmnin")]
    public class HorariosProfissionaisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HorariosProfissionaisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CreateHorarioProfissionalRequest request,CancellationToken cancellationToken)
        {
            var command = new InsertHorarioProfissionalCommand(
                request.ProfissionalId,
                request.DiaSemana,
                request.HoraInicio,
                request.HoraFim);

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("profissional/{profissionalId:guid}")]
        public async Task<IActionResult> GetByProfissional(Guid profissionalId, CancellationToken cancellationToken)
        {
            var query = new GetHorariosByProfissionalQuery(profissionalId);

            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
