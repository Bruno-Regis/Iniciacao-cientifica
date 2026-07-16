using DoctorPilot.Application.Commands.UsuariosCommands.InsertUsuarios;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/usuarios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertUsuariosCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(Post), new { id = result.Data }, result);
        }
    }
}
