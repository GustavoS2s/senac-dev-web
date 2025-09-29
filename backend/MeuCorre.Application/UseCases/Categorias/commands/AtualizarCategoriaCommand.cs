using MediatR;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Categorias.commands
{
    public class AtualizarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "Id da categoria é obrigatório.")]
        public required Guid CategoriaId { get; set; }
        [Required(ErrorMessage = "Nome da categoria é obrigatório")]
        public required string Nome { get; set; }
        [Required(ErrorMessage = "Tipo (despesa ou receita) de categoria é obrigatório")]
        public required Tipotransacao Tipo { get; set; }
        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public string? Icone { get; set; }
        
    }

    internal class AtualizarCategoriaCommandHandler : IRequestHandler<AtualizarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public AtualizarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public async Task<(string, bool)> Handle(AtualizarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
            if (categoria == null)
            {
                return ("Categoria não encontrada.", false);
            }

            Guid? usuarioId = categoria.UsuarioId; // Corrected property name to 'UsuarioId'
            var categoriaEstaDuplicada = await _categoriaRepository.NomeExisteParaUsuarioAsync(request.Nome, request.Tipo, usuarioId.GetValueOrDefault());
            if (categoriaEstaDuplicada)
            {
                return ("Já existe uma categoria com esse nome para o mesmo tipo.", false);
            }
            categoria.AtualizarInformacoes(request.Nome, request.Tipo, request.Descricao, request.Cor, request.Icone);
            await _categoriaRepository.AtualizarAsync(categoria);
            return ("Categoria atualizada com sucesso.", true);
        }
    }
}
