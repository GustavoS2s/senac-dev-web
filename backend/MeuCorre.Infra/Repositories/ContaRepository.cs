using MeuCorre.Application.Interfaces;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using MeuCorre.infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.infra.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly MeuDbContext _meuDbContext;
        public ContaRepository(MeuDbContext meuDbContext)
        {
            _meuDbContext = meuDbContext;
        }

        public async Task AdicionarAsync(Conta conta)
        {
            await _meuDbContext.Contas.AddAsync(conta);
            await _meuDbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Conta conta)
        {
            _meuDbContext.Contas.Update(conta);

            await _meuDbContext.SaveChangesAsync();

        }

        public async Task<List<Conta>> BuscarContasPorUsuarioIdAsync(Guid usuarioId)
        {
            return await _meuDbContext.Contas
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId)
        {
            return await _meuDbContext.Contas
               .Where(c => c.UsuarioId == usuarioId && c.Ativo)
               .SumAsync(c => c.Saldo);
        }

        public async Task ExcluirAsync(Conta conta)
        {
            _meuDbContext.Contas.Remove(conta); 
            await _meuDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null)
        {
            return await _meuDbContext.Contas
               .AnyAsync(c => c.UsuarioId == usuarioId
                           && c.Nome == nome
                           && (!contaIdExcluir.HasValue || c.Id != contaIdExcluir.Value));
        }

        public async Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId)
        {
            return await _meuDbContext.Contas.Where(c => c.Id == contaId && (c.UsuarioId == usuarioId)).FirstOrDefaultAsync();
        }

        public async Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo)
        {
            return await _meuDbContext.Contas.Where(c => c.Tipo == tipo && (c.UsuarioId == usuarioId)).ToListAsync();
        }

        public async Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true)
        {
            return await _meuDbContext.Contas.Where(c => c.UsuarioId == usuarioId && (!apenasAtivas || c.Ativo)).ToListAsync();
        }
    }
}
