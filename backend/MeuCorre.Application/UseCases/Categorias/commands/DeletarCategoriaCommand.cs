using MediatR;
using MeuCorre.Domain.interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Categorias.commands
{
    public class DeletarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o Id do usuário")]
        public required Guid Usuarioid { get; set; }
        [Required(ErrorMessage = "É necessário informar o Id da categoria")]
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
            
                return ("Categoria não encontrada.", false);
            
            if(categoria.UsuarioId != request.Usuarioid )
            
                return ("Categoria não pertence ao usuário.", false);

            await _categoriaRepository.RemoverAsync(categoria);
            return ("Categoria deletada com sucesso.", true);
        }
            
        
    }

}
