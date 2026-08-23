using DoctorPilot.Application.Commands.UsuariosCommands.InsertUsuarios;
using DoctorPilot.Application.Queries.UsuariosQueries.GetAllUsuarios;
using DoctorPilot.Application.Queries.UsuariosQueries.GetUsuarioById;
using DoctorPilot.Core.Enums;
using Mediator;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = nameof(RoleEnum.TenantAdmnin))]
        public async Task<IActionResult> Post([FromBody] InsertUsuariosCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(Post), new { id = result.Data }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsuariosQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetUsuarioByIdQuery(id);

            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
