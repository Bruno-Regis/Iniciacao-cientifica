using DoctorPilot.Application.Commands.AlergiasCommands.InsertAlergias;
using DoctorPilot.Application.Commands.AlergiasCommands.UpdateAlergias;
using DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById;
using DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente;
using DoctorPilot.Api.Authorization;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Enums;
using Mediator;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> Post([FromBody] CreateAlergiaRequest request)
        {
            var command = new InsertAlergiaCommand
            {
                PacienteId = request.PacienteId,
                NomeAlergia = request.NomeAlergia,
                Gravidade = (GravidadeEnum)request.Gravidade,
                DataIdentificacao = request.DataIdentificacao,
                Observacoes = request.Observacoes
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet("paciente/{pacienteId}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId)
        {
            var result = await _mediator.Send(new GetAllAlergiasByPacienteQuery(pacienteId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAlergiaByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateAlergiaCommand command)
        {
            command.AlergiaId = id;
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
