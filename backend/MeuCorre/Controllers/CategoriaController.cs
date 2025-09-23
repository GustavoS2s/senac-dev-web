using MediatR;
using MeuCorre.Application.UseCases.Categorias.commands;
using MeuCorre.Application.UseCases.Categorias.Dtos;
using MeuCorre.Application.UseCases.Categorias.Queries;
using MeuCorre.Application.UseCases.Categorias.Querries;
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
        [ProducesResponseType(typeof(CategoriaDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
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

        [HttpDelete]

        public async Task<IActionResult> DeletarCategoria([FromBody] DeletarCategoriaCommand command)
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

        [HttpPatch("ativar/{id}")]
        public async Task<IActionResult> AtivarCategoria(Guid id)
        {


            var command = new AtivarCategoriaCommand { CategoriaId = id };
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
        [HttpPatch("inativar/{id}")]
        public async Task<IActionResult> InativarCategoria(Guid id)
        {
            var command = new InativarCategoriaCommand { CategoriaId = id };
            var (mensagem, sucesso) = await _mediator.Send(command);
            if (sucesso)
            {
                return Ok( mensagem );
            }
            else
            {
                return BadRequest( mensagem );
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterCategoriasPorUsuario( [FromQuery] ListarTodasCategoriasQuery query )
        {
            var categorias = await _mediator.Send(query);
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCategoriaPorId(Guid id)
        {
            var query = new ObterCategoriaQuerry { CategoriaId = id };
            var categoria = await _mediator.Send(query);
            if (categoria == null)
            {
                return NotFound(new { mensagem = "Categoria não encontrada." });
            }
            return Ok(categoria);
        }
    }
}

