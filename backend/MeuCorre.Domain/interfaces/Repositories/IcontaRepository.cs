using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.Interfaces
{
    public interface IContaRepository
    {
        Task<List<Conta>> ObterPorUsuarioAsync(Guid usuarioId, bool apenasAtivas = true);
        Task<List<Conta>> ObterPorTipoAsync(Guid usuarioId, TipoConta tipo);
        Task<Conta?> ObterPorIdEUsuarioAsync(Guid contaId, Guid usuarioId);
        Task<bool> ExisteContaComNomeAsync(Guid usuarioId, string nome, Guid? contaIdExcluir = null);
        Task<decimal> CalcularSaldoTotalAsync(Guid usuarioId);
        Task AdicionarAsync(Conta conta);
        Task<List<Conta>> BuscarContasPorUsuarioIdAsync(Guid usuarioId);
        Task AtualizarAsync(Conta conta);
        Task ExcluirAsync(Conta conta);
    }
}
