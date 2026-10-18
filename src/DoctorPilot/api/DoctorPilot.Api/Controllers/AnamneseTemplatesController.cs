using DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate;
using DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica;
using DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById;
using DoctorPilot.Application.DTOs;
using DoctorPilot.Api.Authorization;
using DoctorPilot.Contracts.AnamneseTemplates;
using Mediator;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = AuthorizationPolicies.TenantRequired)]
        public async Task<IActionResult> Post([FromBody] CreateAnamneseTemplateRequest request)
        {
            var command = new InsertAnamneseTemplateCommand
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Sessoes = request.Sessoes.Select(s => new InsertAnamneseTemplateSessaoDto
                {
                    Pergunta = s.Pergunta,
                    Tipo = s.Tipo,
                    Ordem = s.Ordem,
                    Obrigatoria = s.Obrigatoria,
                    Opcoes = s.Opcoes.Select(o => new InsertAnamneseTemplateOpcaoDto
                    {
                        Texto = o.Texto,
                        Ordem = o.Ordem
                    }).ToList()
                }).ToList()
            };
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
