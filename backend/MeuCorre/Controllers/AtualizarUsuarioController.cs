using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeuCorre.Application.UseCases.Usuarios.Commands;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MeuCorre.Controllers
{
     [ApiController]
    [Route("[controller]")]
    public class AtualizarUsuarioController : 
    {
        private readonly IMediator _mediator;
        public AtualizarUsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarUsuario([FromBody] AtualizarUsuarioCommand command)
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
    }

}