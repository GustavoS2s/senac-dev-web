using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.interfaces.Repositories;
using MeuCorre.Domain.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UseCases.Categorias.commands
{
    public class CriarCategoriaCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "É necessário informar o Id do usuário")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "É necessário informar o nome da categoria")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Tipo da transação (despesa ou receita) é obrigatório ")]
        public required Tipotransacao Tipo { get; set; }
        
        public string? Cor { get; set; }
        public string? Icone { get; set; }
        public string? Descricao { get; set; }
       
    }
    
    internal class CriarCategoriaCommandHandler : IRequestHandler<CriarCategoriaCommand, (string, bool)>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUsuarioRepository usuarioRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public async Task<(string, bool)> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);
            var existe = await _categoriaRepository.NomeExisteParaUsuarioAsync(request.Nome, request.Tipo, request.UsuarioId);
            if(existe)
            {
                return ("Categoria já cadastrada.", false);
            }
            var novaCategoria = new Categoria(request.Nome,request.Tipo, request.Descricao, request.Cor, request.Icone, request.UsuarioId);
            await _categoriaRepository.AdicionarAsync(novaCategoria);
            return ("Categoria criada com sucesso.", true);
        }
    }
}
