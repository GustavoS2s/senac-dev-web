using MediatR;
using MeuCorre.Application.UserCases.Categorias.DTOs;
using MeuCorre.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Categorias.Queries
{
    public class ListarTodasCategoriasQuery : IRequest<IList<CategoriaDto>>
    {
        [Required(ErrorMessage = "É necessário informar o id do usuário")]
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
            
            if(listaCategorias == null)
                return null;

            var categorias = new List<CategoriaDto>();

            foreach (var cat in listaCategorias)
            {
                categorias.Add(new CategoriaDto
                {
                    Nome = cat.Nome,
                    Ativo = cat.Ativo,
                    Descricao = cat.Descricao,
                    TipoTransacao = cat.TipoTransacao,
                    Cor = cat.Cor,
                    Icone = cat.Icone,
                    UltimaAlteracao = cat.DataAtualizacao,
                });
            }

            return categorias;
        }
    }
}


