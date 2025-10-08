using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using MeuCorre.Application.UseCases.Contas.Commands;
using MeuCorre.Application.UseCases.Contas.Queries;

namespace MeuCorre.Api.Controllers
{
    public class ListarContasRequest
    {
        public int? Tipo { get; set; }
        public bool ApenasAtivas { get; set; } = true;
        public string? OrdenarPor { get; set; }
    }

    [ApiController]
    [Route("api/v1/contas")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid ObterUsuarioId()
        {
            return new Guid("88888888-4444-4444-4444-121212121212");
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> Listar([FromQuery] ListarContasRequest request)
        {
            var query = new ListarContasQuery(
                ObterUsuarioId(),        
                request.Tipo,            
                null,                    
                request.ApenasAtivas,    
                request.OrdenarPor,      
                null,                    
                null                     
            );

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ContaDetalheResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var query = new ObterContaQuery(id, ObterUsuarioId());
            var result = await _mediator.Send(query);

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CriarContaResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Criar([FromBody] CriarContaCommand request)
        {
            var command = new CriarContaCommand(
                request.Nome,
                request.Tipo,
                request.SaldoInicial,
                ObterUsuarioId(),       
                request.Limite,
                request.DiaVencimento,
                request.TipoLimite,
                request.Cor,
                request.Icone
            );

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(AtualizarContaResponse), 200)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarContaCommand request)
        {
            if (id != request.ContaId) return BadRequest("O ID da rota e o ID do corpo da requisição não correspondem.");

            request.ContaId = id;
            request.UsuarioId = ObterUsuarioId();

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Excluir(Guid id, [FromQuery] bool confirmar)
        {
            var command = new ExcluirContaCommand
            {
                ContaId = id,
                UsuarioId = ObterUsuarioId(),
                Confirmar = confirmar
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}