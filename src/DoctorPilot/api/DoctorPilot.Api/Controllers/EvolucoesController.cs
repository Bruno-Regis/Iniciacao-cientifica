using DoctorPilot.Application.Commands.EvolucoesCommands.InsertEvolucoes;
using DoctorPilot.Application.Commands.EvolucoesCommands.UpdateEvolucoes;
using DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento;
using DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucoesByPaciente;
using DoctorPilot.Api.Authorization;
using DoctorPilot.Contracts.Evolucoes;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/evolucoes")]
    [ApiController]
    public class EvolucoesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EvolucoesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> Post([FromBody] CreateEvolucaoRequest request)
        {
            var command = new InsertEvolucaoCommand
            {
                AgendamentoId = request.AgendamentoId,
                Conteudo = request.Conteudo
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(
                nameof(GetByAgendamento),
                new { agendamentoId = request.AgendamentoId },
                result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateEvolucaoRequest request)
        {
            var command = new UpdateEvolucaoCommand
            {
                Id = id,
                Conteudo = request.Conteudo
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("agendamento/{agendamentoId}")]
        public async Task<IActionResult> GetByAgendamento(Guid agendamentoId)
        {
            var result = await _mediator.Send(new GetEvolucaoByAgendamentoQuery(agendamentoId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetEvolucoesByPacienteQuery(pacienteId, page, pageSize));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }
    }
}
