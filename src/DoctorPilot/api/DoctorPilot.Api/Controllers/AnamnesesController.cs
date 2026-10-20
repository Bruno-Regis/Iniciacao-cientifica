using DoctorPilot.Application.Queries.AnamnesesQueries.GetAllAnamnesesByPaciente;
using DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById;
using DoctorPilot.Api.Authorization;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/anamneses")]
    [ApiController]
    public class AnamnesesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnamnesesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paciente/{pacienteId}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> GetByPaciente(Guid pacienteId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllAnamnesesByPacienteQuery(pacienteId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalRequired)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAnamneseByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }
    }
}
