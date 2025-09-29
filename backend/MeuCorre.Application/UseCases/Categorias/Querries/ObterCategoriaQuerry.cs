using MediatR;
using MeuCorre.Application.UseCases.Categorias.Dtos;
using MeuCorre.Domain.interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Categorias.Querries
{
    public class ObterCategoriaQuerry : IRequest<CategoriaDto>
    {
        [Required(ErrorMessage = "Informe o Id da categoria")]
        public required Guid CategoriaId { get; set; }

    }
    
    internal class ObterCategoriaQuerryHandler : IRequestHandler<ObterCategoriaQuerry, CategoriaDto>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public ObterCategoriaQuerryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public async Task<CategoriaDto> Handle(ObterCategoriaQuerry request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
            if (categoria == null)
                return null;

            var categoriaDto = new CategoriaDto
            {
                Nome = categoria.Nome,
                Ativo = categoria.Ativo,
                Tipo = categoria.Tipo,
                Cor = categoria.Cor,
                Descricao = categoria.Descricao,
                Icone = categoria.Icone,
                
            };

            return categoriaDto;
        }
    }
}
