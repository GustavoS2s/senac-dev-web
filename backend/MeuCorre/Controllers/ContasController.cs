using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeuCorre.Application.UserCases.Contas.Commands;
using MeuCorre.Application.UserCases.Contas.Queries;
using System;
using System.Threading.Tasks;

namespace MeuCorre.API.Controllers
{
    [ApiController]
    [Route("api/v1/contas")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObterConta), new { id = id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> ListarContas(
            [FromQuery] string? tipo,
            [FromQuery] bool? apenasAtivas,
            [FromQuery] string? ordenarPor)
        {
            var query = new ListarContasQuery
            {
                Tipo = tipo,
                ApenasAtivas = apenasAtivas,
                OrdenarPor = ordenarPor
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterConta(Guid id, [FromQuery] Guid usuarioId)
        {
            try
            {
                var query = new ObterContaQuery
                {
                    ContaId = id,
                    UsuarioId = usuarioId
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarConta(Guid id, [FromQuery] Guid usuarioId, [FromBody] AtualizarContaCommand command)
        {
            if (id != command.ContaId || usuarioId != command.UsuarioId)
                return BadRequest("IDs inconsistentes.");

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/inativar")]
        public async Task<IActionResult> InativarConta(Guid id, [FromQuery] Guid usuarioId)
        {
            try
            {
                var command = new InativarContaCommand
                {
                    ContaId = id,
                    UsuarioId = usuarioId
                };

                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPatch("{id}/reativar")]
        public async Task<IActionResult> ReativarConta(Guid id, [FromQuery] Guid usuarioId)
        {
            try
            {
                var command = new ReativarContaCommand
                {
                    ContaId = id,
                    UsuarioId = usuarioId
                };

                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirConta(Guid id, [FromQuery] Guid usuarioId, [FromQuery] bool confirmar)
        {
            try
            {
                var command = new ExcluirContaCommand
                {
                    ContaId = id,
                    UsuarioId = usuarioId,
                    Confirmar = confirmar
                };

                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
