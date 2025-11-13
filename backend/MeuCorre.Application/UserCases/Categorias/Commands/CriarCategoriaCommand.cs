using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.Domain.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UserCases.Categorias.Commands
{
    public class CriarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o id do usuário")]
        public required Guid UsuarioId { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome da categoria")]
        public required string Nome { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo da transação")]
        public required TipoTransacao TipoTransacao { get; set; }
        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public string? Icone { get; set; }
    }

    internal class CriarCategoriaCommandHandler : IRequestHandler<CriarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUsuarioRepository usuarioRepository)
        {
            _categoriaRepository = categoriaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<(string, bool)> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorId(request.UsuarioId);
            if (usuario == null)
            {
                return ("Usuário inválido", false);
            }

            var existe = await _categoriaRepository.NomeExisteParaUsuarioAsync(request.Nome, request.TipoTransacao, request.UsuarioId);
            if(existe)
            {
                return ("Categoria já cadastrada", false);
            }

            var novaCategoria = new Categoria(request.Nome, request.Descricao, request.TipoTransacao, request.Cor, request.Icone, request.UsuarioId);
            await _categoriaRepository.AdicinarAsync(novaCategoria);
            return ("Categoria criada com sucesso", true);

        }
    }
}
