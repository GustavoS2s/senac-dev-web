using MediatR;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Categorias.Commands
{
    public class AtualizarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "Id da categoria é obrigatória")]
        public required Guid CategoriaId { get; set; }
        [Required(ErrorMessage = "Nome da categoria é obrigatória")]
        public required string Nome { get; set; }
        [Required(ErrorMessage = "Tipo da transação é obrigatório")]
        public required TipoTransacao TipoTransacao { get; set; }
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
                return ("Categoria não encontrada", false);
            }

            var categoriaEstaDuplicada = await _categoriaRepository.NomeExisteParaUsuarioAsync(request.Nome, request.TipoTransacao, categoria.UsuarioId.GetValueOrDefault());

            if(categoriaEstaDuplicada)
            {
                return ("Já existe uma categoria cadastrada com esses dados", false);
            }

            categoria.AtualizarInformacoes(request.Nome, request.Descricao, request.Cor, request.Icone, request.TipoTransacao);
            await _categoriaRepository.AtualizarAsync(categoria);
            return ("Categoria atualizada com sucesso", true);
        }
    }
}
