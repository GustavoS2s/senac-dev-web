using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UseCases.Usuarios.Commands
{
    class AtualizarUsuarioCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres.")]
        public required string Senha { get; set; }
        public DateTime DataNascimento { get; set; }

    }

    internal class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, (string, bool)>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AtualizarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<(string, bool)> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorEmail(request.Email);
            if (usuarioExistente != null)
            {
                return ("Usuário com este email já existe.", false);
            }

            var novoUsuario = new Usuario(
                 request.Nome,
                 request.Email,
                 request.Senha,
                request.DataNascimento,
                true);
            await _usuarioRepository.AtualizarUsuarioAsync(novoUsuario);
            return ("Usuário criado com sucesso.", true);

        }

        public Task<(string, bool)> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
