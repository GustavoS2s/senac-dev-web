using MediatR;
using MeuCorre.Application.UserCases.Usuarios.Commands;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UserCases.Usuarios.Commands
{
    public class CriarUsuarioCommand : IRequest<(string, bool)>
    {
        [Required(ErrorMessage = "Nome é obrigatorio")]
        public required string Nome { get; set; }
        [Required(ErrorMessage = "Email é obrigatorio")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatorio")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public required string Senha { get; set; }
        [Required(ErrorMessage = "Data de nascimento é obrigatorio")]
        public required DateTime DataNascimento { get; set; }
    }

    internal class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, (string, bool)>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<(string, bool)> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorEmail(request.Email);
            if (usuarioExistente != null)
            {
                return ("Já existe usuário cadastrado com este email.", false);
            }

            var novoUsuario = new Usuario(request.Nome, request.Email, request.Senha, request.DataNascimento, true);

            await _usuarioRepository.CriarUsuarioAsync(novoUsuario);
            return ("Usuário criado com sucesso", true);
        }
    }

}