using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseFromTemplate;
using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseManual;
using DoctorPilot.Contracts.Anamneses;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/anamneses")]
    [ApiController]
    public class AnamneseCreationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnamneseCreationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("from-template")]
        public async Task<IActionResult> PostFromTemplate(
            [FromBody] CreateAnamneseFromTemplateRequest request)
        {
            var command = new InsertAnamneseFromTemplateCommand
            {
                PacienteId = request.PacienteId,
                TemplateId = request.TemplateId,
                Observacoes = request.Observacoes,
                Respostas = request.Respostas.Select(r => new InsertAnamneseFromTemplateRespostaDto
                {
                    PerguntaId = r.PerguntaId,
                    Resposta = r.Resposta,
                    OpcaoIds = r.OpcaoIds.ToList()
                }).ToList()
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(AnamnesosController.GetById),
                "Anamnesos",
                new { id = result.Data },
                result);
        }

        [HttpPost("manual")]
        public async Task<IActionResult> PostManual([FromBody] CreateAnamneseManualRequest request)
        {
            var command = new InsertAnamneseManualCommand
            {
                PacienteId = request.PacienteId,
                NomeDaAnamnese = request.NomeDaAnamnese,
                Observacoes = request.Observacoes,
                Sessoes = request.Sessoes.Select(s => new InsertAnamneseManualSessaoDto
                {
                    Pergunta = s.Pergunta,
                    Tipo = s.Tipo,
                    Ordem = s.Ordem,
                    Resposta = s.Resposta,
                    Opcoes = s.Opcoes.Select(o => new InsertAnamneseManualOpcaoDto
                    {
                        Texto = o.Texto,
                        Ordem = o.Ordem,
                        Selecionada = o.Selecionada
                    }).ToList()
                }).ToList()
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(AnamnesosController.GetById),
                "Anamnesos",
                new { id = result.Data },
                result);
        }
    }
}
