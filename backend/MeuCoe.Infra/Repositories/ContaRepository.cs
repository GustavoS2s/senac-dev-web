using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.Interfaces;
using MeuCorre.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MeuCorre.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly MeuDbContext _meuDbContext;

        public ContaRepository(MeuDbContext context)
        {
            _meuDbContext = context;
        }


        public async Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true)
        {
            var query = _meuDbContext.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId);

            if (apenasAtivas)
            {
                query = query.Where(c => c.Ativo);
            }

            return await query
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo)
        {
            return await _meuDbContext.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId && c.Tipo == tipo && c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId)
        {
            return await _meuDbContext.Contas
                .Where(c => c.Id == contaId && c.UsuarioId == usuarioId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null)
        {
            var query = _meuDbContext.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId && c.Nome == nome);

            if (contaIdExcluir.HasValue)
            {
                query = query.Where(c => c.Id != contaIdExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId)
        {
            var saldo = await _meuDbContext.Contas
                .Where(c => c.UsuarioId == usuarioId && c.Ativo)
                .SumAsync(c => c.Saldo);

            return saldo;
        }

            public async Task AdicionarAsync(Conta entity)
            {
                await _meuDbContext.Contas.AddAsync(entity);
            }

            public async Task SalvarAsync()
            {
               await _meuDbContext.SaveChangesAsync();
            }

            public async Task Atualizar(Conta entity)
            {
                _meuDbContext.Contas.Update(entity);
            await Task.CompletedTask;
            }
            
            public async Task Excluir(Conta entity)
            {
            _meuDbContext.Contas.Remove(entity);
            await Task.CompletedTask;
            }

    }
}