using MeuCorre.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task CriarUsuarioAsync(Usuario usuario);
        Task AtualizarUsuarioAsync(Usuario usuario);
        Task RemoverUsuarioAsync(Usuario usuario);
        Task<Usuario?> ObterUsuarioPorEmail(string email);
    }
}
