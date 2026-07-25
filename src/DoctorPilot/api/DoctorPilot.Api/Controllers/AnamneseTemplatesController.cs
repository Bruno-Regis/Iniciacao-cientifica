using DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate;
using DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica;
using DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/anamnese-templates")]
    [ApiController]
    public class AnamneseTemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnamneseTemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertAnamneseTemplateCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet("clinica/{clinicaId}")]
        public async Task<IActionResult> GetByClinica(Guid clinicaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllAnamneseTemplatesByClinicaQuery(clinicaId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAnamneseTemplateByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }
    }
}
