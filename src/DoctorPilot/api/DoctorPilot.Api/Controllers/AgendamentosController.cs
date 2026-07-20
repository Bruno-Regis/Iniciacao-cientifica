using DoctorPilot.Application.Commands.AgendamentosCommands.InsertAgendamentos;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/agendamentos")]
    [ApiController]
    public class AgendamentosController : ControllerBase 
    {
        private readonly IMediator _mediator;

        public AgendamentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertAgendamentoCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAgendamentoByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var result = await _mediator.Send(new GetAllAgendamentosByPeriodoQuery(inicio, fim));
            return Ok(result);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllAgendamentosByPacienteQuery(pacienteId, page, pageSize));
            return Ok(result);
        }

    }
}
