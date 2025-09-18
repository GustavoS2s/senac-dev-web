using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.interfaces.Repositories;
using MeuCorre.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MeuCorre.Infra.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly MeuDbContext _MeuDbcontext;
        public CategoriaRepository(MeuDbContext context)
        {
            _MeuDbcontext = context;
        }

        async Task ICategoriaRepository.AdicionarAsync(Categoria categoria)
        {
            _MeuDbcontext.Categorias.Add(categoria);
            await _MeuDbcontext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Categoria categoria)
        {
            _MeuDbcontext.Categorias.Update(categoria);
            await _MeuDbcontext.SaveChangesAsync() ;
        }

        public async Task<bool>ExisteAsync(Guid categoriaid)
        {
           var existe = await _MeuDbcontext.Categorias.AnyAsync(c => c.Id == categoriaid);
            return existe;
        }

        public async Task<IEnumerable<Categoria>> ListarTodasPorUsuarioAsync(Guid usuarioId)
        {
            var listaCategorias =  _MeuDbcontext.Categorias.Where(c=> c.UsuarioId == usuarioId);
            return await listaCategorias.ToListAsync();
            
        }

        public async Task<bool> NomeExisteParaUsuarioAsync(string nome, Tipotransacao tipo, Guid usuarioId)
        {
            var existe = await _MeuDbcontext.Categorias.AnyAsync(c => c.Nome == nome && c.UsuarioId == usuarioId && c.Tipo == tipo);
            return existe;
        }

        public async Task<Categoria?> ObterPorIdAsync(Guid categoriaid)
        {
            var categoria = await _MeuDbcontext.Categorias.FindAsync(categoriaid);
            return categoria;
        }

        async Task ICategoriaRepository.RemoverAsync(Categoria categoria)
        {
            _MeuDbcontext.Categorias.Remove(categoria);
            await _MeuDbcontext.SaveChangesAsync();
        }

        public Task<bool> NomeExisteParaUsuarioAsync(string nome, Tipotransacao tipo, Guid? usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
