using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.Interfaces;
using System.Text.RegularExpressions; 

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public record CriarContaResponse(Guid Id, string Nome, TipoConta Tipo, decimal Saldo);

    public record CriarContaCommand(
        string Nome,
        TipoConta Tipo,
        decimal SaldoInicial,
        Guid UsuarioId,
        decimal? Limite,
        int? DiaVencimento,
        TipoLimite? TipoLimite,
        string? Cor,
        string? Icone
    ) : IRequest<CriarContaResponse>;

    public class CriarContaCommandHandler : IRequestHandler<CriarContaCommand, CriarContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public CriarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<CriarContaResponse> Handle(CriarContaCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) || request.Nome.Length < 2 || request.Nome.Length > 50)
            {
                throw new ApplicationException("O nome da conta deve ter entre 2 e 50 caracteres.");
            }

            if (!Enum.IsDefined(typeof(TipoConta), request.Tipo))
            {
                throw new ApplicationException("O tipo de conta é inválido.");
            }

            if (request.Tipo == TipoConta.CartaoCredito)
            {
                if (request.Limite == null || request.Limite <= 0)
                {
                    throw new ApplicationException("O limite deve ser positivo e é obrigatório para Cartões de Crédito.");
                }

                if (request.DiaVencimento == null || request.DiaVencimento < 1 || request.DiaVencimento > 31)
                {
                    throw new ApplicationException("O dia de vencimento é obrigatório e deve ser entre 1 e 31 para Cartões de Crédito.");
                }
            }

            if (!string.IsNullOrEmpty(request.Cor) && !Regex.IsMatch(request.Cor, "^#([A-Fa-f0-9]{6})$"))
            {
                throw new ApplicationException("A cor deve estar no formato hexadecimal (#RRGGBB).");
            }

            var nomeJaExiste = await _contaRepository.ExisteContaComNomeAsync(request.UsuarioId, request.Nome);
            if (nomeJaExiste)
            {
                throw new ApplicationException($"Já existe uma conta com o nome '{request.Nome}' para este usuário.");
            }

            int? diaFechamentoFinal = request.DiaVencimento;

            if (request.Tipo == TipoConta.CartaoCredito && request.DiaVencimento.HasValue)
            {
                diaFechamentoFinal = request.DiaVencimento.Value - 10;
                if (diaFechamentoFinal <= 0) diaFechamentoFinal += 30;
            }

            decimal saldoFinal = request.SaldoInicial;

            var conta = new Conta(
                nome: request.Nome,
                tipo: request.Tipo,
                saldo: saldoFinal,
                usuarioId: request.UsuarioId,
                limite: request.Limite,
                diaFechamento: diaFechamentoFinal,
                diaVencimento: request.DiaVencimento,
                tipoLimite: request.TipoLimite,
                cor: request.Cor,
                icone: request.Icone
            );

            await _contaRepository.AdicionarAsync(conta);
            await _contaRepository.SalvarAsync();

            return new CriarContaResponse(
                Id: conta.Id,
                Nome: conta.Nome,
                Tipo: conta.Tipo,
                Saldo: conta.Saldo
            );
        }
    }
}