using DoctorPilot.Application.Commands.AgendamentosCommands.CancelarAgendamento;
using DoctorPilot.Application.Commands.AgendamentosCommands.ConfirmarAgendamento;
using DoctorPilot.Application.Commands.AgendamentosCommands.InsertAgendamentos;
using DoctorPilot.Application.Commands.AgendamentosCommands.UpdateAgendamento;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentosElegiveisParaEvolucaoByPaciente;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo;
using DoctorPilot.Application.Queries.AgendamentosQueries.GetAvailableSlots;
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
        public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] Guid? profissionalId = null)
        {
            var result = await _mediator.Send(new GetAllAgendamentosByPeriodoQuery(inicio, fim, profissionalId));
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllAgendamentosByPacienteQuery(pacienteId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("paciente/{pacienteId}/elegiveis-evolucao")]
        public async Task<IActionResult> GetElegiveisParaEvolucao(Guid pacienteId)
        {
            var result = await _mediator.Send(
                new GetAgendamentosElegiveisParaEvolucaoByPacienteQuery(pacienteId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateAgendamentoCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            var result = await _mediator.Send(new CancelarAgendamentoCommand(id));
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid id)
        {
            var result = await _mediator.Send(new ConfirmarAgendamentoCommand(id));
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(Guid profissionalId, DateTime inicio, DateTime fim, int duracaoMinutos, CancellationToken cancellationToken)
        {
            var query = new GetAvailableSlotsQuery(profissionalId, inicio, fim, duracaoMinutos);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
