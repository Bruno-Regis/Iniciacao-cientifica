using DoctorPilot.Application.Commands.AlergiasCommands.InsertAlergias;
using DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById;
using DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/alergias")]
    [ApiController]
    public class AlergiasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlergiasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertAlergiaCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId)
        {
            var result = await _mediator.Send(new GetAllAlergiasByPacienteQuery(pacienteId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAlergiaByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

    }
}
