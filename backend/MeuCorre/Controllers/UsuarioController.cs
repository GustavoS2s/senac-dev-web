using MediatR;
using MeuCorre.Application.UseCases.Usuarios.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MeuCorre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController
    {
        private readonly IMediator _mediator;
        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioCommand command)
        {
            var (mensagem, sucesso) = await _mediator.Send(command);
            if (sucesso)
            {
                return Ok(new { mensagem });
            }
            else
            {
                return Conflict(mensagem);
            }

        }

        private IActionResult Conflict(string mensagem)
        {
            throw new NotImplementedException();
        }

        private IActionResult Ok(object value)
        {
            throw new NotImplementedException();
        }
    

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] AtualizarUsuarioCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("O ID do usuário na URL não corresponde ao ID no corpo da solicitação.");
            }
            var (mensagem, sucesso) = await _mediator.Send(command);
            if (sucesso)
            {
                return Ok(new { mensagem });
            }
            else
            {
                return NotFound(mensagem);
            }
        }

        private IActionResult NotFound(string mensagem)
        {
            throw new NotImplementedException();
        }

        private IActionResult BadRequest(string v)
        {
            throw new NotImplementedException();
        }
    }
}
