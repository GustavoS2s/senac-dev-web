using MediatR;
using MeuCorre.Application.UseCases.Categorias.commands;
using Microsoft.AspNetCore.Mvc;

namespace MeuCorre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]

        public async Task<IActionResult> CriarCategoria([FromBody] CriarCategoriaCommand command)
        {
            var (mensagem, sucesso) = await _mediator.Send(command);
            if (sucesso)
            {
                return Ok(new { mensagem });
            }
            else
            {
                return BadRequest(new { mensagem });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarCategoria([FromBody] AtualizarCategoriaCommand command)
        {
            var (mensagem, sucesso) = await _mediator.Send(command);
            if (sucesso)
            {
                return Ok(new { mensagem });
            }
            {
                return BadRequest(new { mensagem });
            }
        }




    }
}
