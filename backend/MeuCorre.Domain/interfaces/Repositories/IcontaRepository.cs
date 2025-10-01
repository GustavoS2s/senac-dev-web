using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns; 

namespace MeuCorre.Domain.Interfaces
{
    public interface IContaRepository 
    {

        Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true);

        Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo);

        Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId);

        Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null);

        Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId);
        Task SalvarAsync();
        Task AdicionarAsync(Conta conta);
    }
}