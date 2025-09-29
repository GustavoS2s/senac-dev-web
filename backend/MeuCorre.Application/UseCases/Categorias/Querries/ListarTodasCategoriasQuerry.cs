using MediatR;
using MeuCorre.Application.UseCases.Categorias.Dtos;
using MeuCorre.Domain.interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UseCases.Categorias.Queries
{
    public class ListarTodasCategoriasQuery : IRequest<IList<CategoriaDto>>
    {
        [Required(ErrorMessage = "Informe o Id da categoria")]
        public required Guid UsuarioId { get; set; }
    }

    internal class ListarTodasCategoriasQueryHandler : IRequestHandler<ListarTodasCategoriasQuery, IList<CategoriaDto>>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public ListarTodasCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IList<CategoriaDto>> Handle(ListarTodasCategoriasQuery request, CancellationToken cancellationToken)
        {
            var listaCategorias = await _categoriaRepository.ListarTodasPorUsuarioAsync(request.UsuarioId);

            if (listaCategorias == null)
                return null;

            var categorias = new List<CategoriaDto>();

            foreach (var cat in listaCategorias)
            {
                categorias.Add(new CategoriaDto
                {
                    Nome = cat.Nome,
                    Ativo = cat.Ativo,
                    Tipo = cat.Tipo,
                    Cor = cat.Cor,
                    Descricao = cat.Descricao,
                    Icone = cat.Icone,
                    UltimaAlteracao = cat.DataAtualizacao
                });
            }

            return categorias;
        }
    }

}