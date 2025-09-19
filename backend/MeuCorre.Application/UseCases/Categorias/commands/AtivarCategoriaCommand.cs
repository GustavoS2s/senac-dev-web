using MediatR;
using MeuCorre.Domain.interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UseCases.Categorias.commands
{
    public class AtivarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o Id da categoria")]
        public required Guid CategoriaId { get; set; }
    }

    internal class AtivarCategoriaCommandHandler : IRequestHandler<AtivarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public AtivarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public async Task<(string, bool)> Handle(AtivarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);

            categoria.Ativar();

            await _categoriaRepository.AtualizarAsync(categoria);
            return ("Categoria ativada com sucesso.", true);
        }
    }
}

