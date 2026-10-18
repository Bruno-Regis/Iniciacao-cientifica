using DoctorPilot.Application.Commands.PacientesCommands.InsertPacientes;
using DoctorPilot.Application.Commands.PacientesCommands.UpdatePcientes;
using DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica;
using DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById;
using DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/pacientes")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertPacientesCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByClinica([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllPacientesByClinicaQuery(page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPacienteByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] Guid clinicaId, [FromQuery] string termo, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new SearchPacientesQuery(clinicaId, termo, page, pageSize));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePacientesCommand command)
        {
            command.PacienteId = id;
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
