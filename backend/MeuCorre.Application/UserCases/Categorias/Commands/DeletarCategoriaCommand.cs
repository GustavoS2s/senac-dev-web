using MediatR;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Categorias.Commands
{
    public class DeletarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o id do usuário")]
        public required Guid UsuarioId { get; set; }
        [Required(ErrorMessage = "É necessário informar o id da categoria")]
        public required Guid CategoriaId { get; set; }
    }

    internal class DeletarCategoriaCommandHandler : IRequestHandler<DeletarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public DeletarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<(string, bool)> Handle(DeletarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
            if (categoria == null)
            {
                return ("Categoria não encontrada", false);
            }

            if (categoria.UsuarioId != request.UsuarioId)
            {
                return ("Você não tem permissão para deletar essa categoria", false);
            }

            await _categoriaRepository.RemoverAsync(categoria);
            return ("Categoria deletada com sucesso", true);
        }
    }
}
