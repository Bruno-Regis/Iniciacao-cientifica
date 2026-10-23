using DoctorPilot.Api.Authorization;
using DoctorPilot.Application.Commands.TranscricoesCommands.TranscribeAppointment;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{

    [ApiController]
    [Route("api/transcricoes")]
    [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
    public class TranscricoesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranscricoesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("agendamentos/{agendamentoId:guid}")]
        public async Task<IActionResult> TranscribeAppointment(
            Guid agendamentoId,
            IFormFile audio,
            CancellationToken cancellationToken)
        {
            await using var stream = audio.OpenReadStream();

            var command = new TranscribeAppointmentCommand
            {
                AgendamentoId = agendamentoId,
                Audio = stream,
                FileName = audio.FileName
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }
    } 
}
