using MediatR;
using MeuCorre.Domain.interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UseCases.Categorias.commands
{
    public class InativarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o Id da categoria")]
        public required Guid CategoriaId { get; set; }
    }

    internal class InativarCategoriaCommandHandler : IRequestHandler<InativarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public InativarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public async Task<(string, bool)> Handle(InativarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);

            categoria.Ativar();

            await _categoriaRepository.AtualizarAsync(categoria);
            return ("Categoria desativada com sucesso.", true);
        }
    }
}
