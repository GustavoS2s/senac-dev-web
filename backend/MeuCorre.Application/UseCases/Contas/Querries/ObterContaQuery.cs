using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.Interfaces;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    public record ContaDetalheResponse(
        Guid Id,
        string Nome,
        TipoConta Tipo,
        decimal Saldo,
        Guid UsuarioId,
        decimal? Limite,
        int? DiaFechamento,
        int? DiaVencimento,
        TipoLimite? TipoLimite,
        string? Cor,
        string? Icone,
        bool Ativo,
        DateTime DataCadastro,
        int TotalTransacoes,
        decimal TotalReceitas,
        decimal TotalDespesas
    );

    public record ObterContaQuery(
        Guid ContaId,
        Guid UsuarioId 
    ) : IRequest<ContaDetalheResponse>;

    public class ObterContaQueryHandler : IRequestHandler<ObterContaQuery, ContaDetalheResponse>
    {
        private readonly IContaRepository _contaRepository;

        public ObterContaQueryHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ContaDetalheResponse> Handle(ObterContaQuery request, CancellationToken cancellationToken)
        {

            Conta? conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
            {
                throw new ApplicationException($"Conta com ID {request.ContaId} não encontrada ou não pertence ao usuário.");
            }

            return new ContaDetalheResponse(
                Id: conta.Id,
                Nome: conta.Nome,
                Tipo: conta.Tipo,
                Saldo: conta.Saldo,
                UsuarioId: conta.UsuarioId,
                Limite: conta.Limite,
                DiaFechamento: conta.DiaFechamento,
                DiaVencimento: conta.DiaVencimento,
                TipoLimite: conta.TipoLimite,
                Cor: conta.Cor,
                Icone: conta.Icone,
                Ativo: conta.Ativo,
                DataCadastro: conta.DataCriacao,
                TotalTransacoes: 0,
                TotalReceitas: 0,
                TotalDespesas: 0
            );
        }
    }
}