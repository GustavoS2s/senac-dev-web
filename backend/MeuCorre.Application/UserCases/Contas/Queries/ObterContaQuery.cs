using MediatR;
using MeuCorre.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Queries
{
    public class ObterContaQuery : IRequest<ContaDetalheResponse> // Added IRequest<ContaDetalheResponse> implementation
    {
        public required Guid UsuarioId { get; set; }
        public required Guid ContaId { get; set; }
    }

    public class ContaDetalheResponse
    {
        public required Guid UsuarioId { get; set; }
        public required Guid ContaId { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Saldo { get; set; }

        public int QuantidadeTransacoes { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
    }

    internal class ObterContaQueryHandler : IRequestHandler<ObterContaQuery, ContaDetalheResponse>
    {
        private readonly IContaRepository _contaRepository;

        public ObterContaQueryHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ContaDetalheResponse> Handle(ObterContaQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada.");
            }

            int quantidade = 0;
            int receitas = 0;
            int despesas = 0;

            return new ContaDetalheResponse
            {
                ContaId = conta.Id,
                UsuarioId = conta.UsuarioId,
                Nome = conta.Nome,
                Tipo = conta.Tipo.ToString(),
                Saldo = conta.Saldo,
                QuantidadeTransacoes = quantidade,
                TotalReceitas = receitas,
                TotalDespesas = despesas
            };
        }
    }
}
