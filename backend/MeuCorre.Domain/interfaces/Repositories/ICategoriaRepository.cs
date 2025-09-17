using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;

namespace MeuCorre.Domain.interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria?>ObterPorIdAsync(Guid categoriaid);
        Task<IEnumerable<Categoria>> ListarTodasPorUsuarioAsync(Guid usuarioId);

        Task<bool> ExisteAsync(Guid categoriaid);
        Task<bool> NomeExisteParaUsuarioAsync(string nome, Tipotransacao tipo, Guid usuarioId);
        Task AdicionarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task RemoverAsync(Categoria categoria);
    }
}
