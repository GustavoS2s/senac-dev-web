using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.Interfaces;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    public record ContaResumoResponse(
        Guid Id,
        string Nome,
        TipoConta Tipo,
        decimal Saldo,
        string Cor,
        string Icone,
        decimal? LimiteTotal,
        decimal? LimiteDisponivel 
    );

    public record ListarContasQuery(
        Guid UsuarioId, 
        TipoConta? FiltrarPorTipo,
        bool ApenasAtivas = true,
        string? OrdenarPor = "Nome" 
    ) : IRequest<List<ContaResumoResponse>>;

    public class ListarContasQueryHandler : IRequestHandler<ListarContasQuery, List<ContaResumoResponse>>
    {
        private readonly IContaRepository _contaRepository;

        public ListarContasQueryHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<List<ContaResumoResponse>> Handle(ListarContasQuery request, CancellationToken cancellationToken)
        {
            List<Conta> contas;

            if (request.FiltrarPorTipo.HasValue)
            {
                contas = await _contaRepository.ObterPorTipoAsync(request.UsuarioId, request.FiltrarPorTipo.Value);
            }
            else
            {
                contas = await _contaRepository.ObterPorUsuarioAsync(request.UsuarioId, request.ApenasAtivas);
            }

            if (request.OrdenarPor?.Equals("Tipo", StringComparison.OrdinalIgnoreCase) == true)
            {
                contas = contas.OrderBy(c => c.Tipo).ToList();
            }
            else 
            {
                contas = contas.OrderBy(c => c.Nome).ToList();
            }

            var response = contas.Select(c =>
            {
                decimal? limiteDisponivel = null;

                if (c.Tipo == TipoConta.CartaoCredito && c.Limite.HasValue)
                {
                    limiteDisponivel = c.Limite.Value - c.Saldo;
                }

                return new ContaResumoResponse(
                    Id: c.Id,
                    Nome: c.Nome,
                    Tipo: c.Tipo,
                    Saldo: c.Saldo,
                    Cor: c.Cor,
                    Icone: c.Icone,
                    LimiteTotal: c.Limite,
                    LimiteDisponivel: limiteDisponivel
                );
            }).ToList();

            return response;
        }
    }
}