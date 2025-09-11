using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MeuCorre.Application.UseCases.Usuarios.Commands
{
    public class CriarUsuarioCommand : IRequest<(string,bool)>
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

    internal class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, (string, bool)>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<(string,bool)> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorEmail(request.Email);
            if (usuarioExistente != null)
            {
                return ("Usuário com este email já existe.", false);
            }

            var ano = DateTime.Now.Year;
            var idade = ano - request.DataNascimento.Year;
            if(idade < 13)
            {
                return ("Usuário deve ser maior de 13 anos.", false);
            }
            var novoUsuario = new Usuario(
                 request.Nome,
                 request.Email,
                 request.Senha,
                request.DataNascimento,
                true);
            await _usuarioRepository.CriarUsuarioAsync(novoUsuario);
            return ("Usuário criado com sucesso.", true);

        }
    }
}
