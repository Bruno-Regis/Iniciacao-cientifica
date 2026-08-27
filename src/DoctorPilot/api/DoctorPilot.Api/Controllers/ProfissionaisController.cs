using DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/profissionais")]
    [ApiController]
    public class ProfissionaisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfissionaisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertProfissionaisCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByClinica([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllProfissionaisByClinicaQuery(page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProfissionalByIdQuery(id));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
