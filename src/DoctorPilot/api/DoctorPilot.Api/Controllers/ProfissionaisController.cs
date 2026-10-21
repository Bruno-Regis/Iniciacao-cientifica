using DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica;
using DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Api.Authorization;
using Mediator;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = AuthorizationPolicies.TenantAdministration)]
        public async Task<IActionResult> Post([FromBody] CreateProfissionalRequest request)
        {
            var command = new InsertProfissionaisCommand
            {
                Email = request.Email,
                Senha = request.Senha,
                Nome = request.Nome,
                Profissao = request.Profissao,
                FotoUrl = request.FotoUrl,
                Crm = request.Crm
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.ProfessionalDirectoryAccess)]
        public async Task<IActionResult> GetAllByClinica([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllProfissionaisByClinicaQuery(page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = AuthorizationPolicies.TenantAdministration)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProfissionalByIdQuery(id));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
