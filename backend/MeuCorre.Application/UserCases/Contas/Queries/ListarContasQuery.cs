using MediatR;
using MeuCorre.Application.Interfaces;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ContaResumoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal SaldoAtual { get; set; }
    public decimal? LimiteDisponivel { get; set; }
}

namespace MeuCorre.Application.UserCases.Contas.Queries
{
    public class ListarContasQuery : IRequest<List<ContaResumoResponse>>
    {
        [Required(ErrorMessage = "É necessário informar o id do usuário")]
        public Guid UsuarioId { get; set; }

        public string? FiltrarPorTipo { get; set; }

        public bool? ApenasAtivas { get; set; }

        public string? OrdenarPor { get; set; }
        public string? Tipo { get; set; }
    }


    public class ListarContasQueryHandler : IRequestHandler<ListarContasQuery, List<ContaResumoResponse>>
    {
        private readonly IContaRepository _contaRepository;

        public ListarContasQueryHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<List<ContaResumoResponse>> Handle(ListarContasQuery request, CancellationToken cancellationToken)
        {

            var contas = await _contaRepository.BuscarContasPorUsuarioIdAsync(request.UsuarioId);

            if (contas == null || !contas.Any())
                return new List<ContaResumoResponse>();

            var query = contas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.FiltrarPorTipo) &&
                Enum.TryParse<TipoConta>(request.FiltrarPorTipo, true, out var tipoConta))
            {
                query = query.Where(c => c.Tipo == tipoConta);
            }

            if (request.ApenasAtivas.HasValue)
            {
                query = query.Where(c => c.Ativo == request.ApenasAtivas.Value);
            }

            var contasList = query.ToList();

            var responseList = contasList.Select(c =>
            {
                decimal? limiteDisponivel = null;

                if (c.Tipo == TipoConta.CartaoCredito && c.LimiteValor.HasValue)
                    limiteDisponivel = c.LimiteValor.Value - c.Saldo;

                return new ContaResumoResponse
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Tipo = c.Tipo.ToString(), // convertendo enum para string
                    SaldoAtual = c.Saldo,
                    LimiteDisponivel = limiteDisponivel
                };
            }).ToList();


            if (!string.IsNullOrWhiteSpace(request.OrdenarPor))
            {
                responseList = request.OrdenarPor.ToLowerInvariant() switch
                {
                    "nomeasc" => responseList.OrderBy(r => r.Nome).ToList(),
                    "nomedesc" => responseList.OrderByDescending(r => r.Nome).ToList(),
                    "saldoasc" => responseList.OrderBy(r => r.SaldoAtual).ToList(),
                    "saldodesc" => responseList.OrderByDescending(r => r.SaldoAtual).ToList(),
                    _ => responseList.OrderBy(r => r.Nome).ToList(),
                };
            }

            return responseList;
        }
    }
}
