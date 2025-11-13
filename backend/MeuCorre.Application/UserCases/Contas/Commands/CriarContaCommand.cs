using MediatR;
using MeuCorre.Application.Interfaces;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Commands
{
    public class CriarContaCommand : IRequest<CriarContaResponse>
    {
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O nome da conta é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres.")]
        public string Nome { get; set; } = default!;

        [Required(ErrorMessage = "O tipo da conta é obrigatório.")]
        public TipoConta Tipo { get; set; }

        public decimal Saldo { get; set; }
        public decimal? Limite { get; set; }
        public int? DiaVencimento { get; set; }
        public int? DiaFechamento { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class CriarContaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public TipoConta Tipo { get; set; }
        public decimal Saldo { get; set; }
        public decimal? Limite { get; set; }
        public int? DiaVencimento { get; set; }
        public int? DiaFechamento { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }
    }

    internal class CriarContaCommandHandler : IRequestHandler<CriarContaCommand, CriarContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public CriarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<CriarContaResponse> Handle(CriarContaCommand request, CancellationToken cancellationToken)
        {
            // 1. Verificar se já existe conta com o mesmo nome para o mesmo usuário
            bool existeConta = await _contaRepository.ExisteContaComNomeAsync(request.UsuarioId, request.Nome);
            if (existeConta)
                throw new ApplicationException("Já existe uma conta com esse nome para o usuário.");

            // 2. Validações para tipo Cartão de Crédito
            if (request.Tipo == TipoConta.CartaoCredito)
            {
                if (!request.Limite.HasValue)
                    throw new ApplicationException("Limite é obrigatório para contas do tipo Cartão de Crédito.");

                if (!request.DiaVencimento.HasValue || request.DiaVencimento < 1 || request.DiaVencimento > 31)
                    throw new ApplicationException("Dia de vencimento deve ser entre 1 e 31 para cartões de crédito.");
            }

            // 3. Validar formato da cor (hex #RRGGBB)
            if (!string.IsNullOrWhiteSpace(request.Cor))
            {
                var regex = new Regex(@"^#([0-9A-Fa-f]{6})$");
                if (!regex.IsMatch(request.Cor))
                    throw new ApplicationException("A cor informada está em um formato inválido. Use o formato hexadecimal: #RRGGBB");
            }

            // 4. Ajustar saldo (positivo)
            var saldoAjustado = request.Saldo < 0 ? request.Saldo * -1 : request.Saldo;

            // 5. Calcular dia de fechamento se for cartão e não for informado
            int? diaFechamento = request.DiaFechamento;
            if (request.Tipo == TipoConta.CartaoCredito && !diaFechamento.HasValue)
            {
                diaFechamento = request.DiaVencimento.Value - 10;
                if (diaFechamento <= 0)
                    diaFechamento += 30;
            }

            // 6. Criar entidade Conta
            var conta = new Domain.Entities.Conta
            {
                UsuarioId = request.UsuarioId,
                Nome = request.Nome,
                Tipo = request.Tipo,
                Saldo = saldoAjustado,
                Limite = request.Limite.HasValue ? (TipoLimite?)request.Limite.Value : null,
                DiaVencimento = request.DiaVencimento.HasValue
                    ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, request.DiaVencimento.Value)
                    : null,
                DiaFechamento = diaFechamento.HasValue
                    ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, diaFechamento.Value)
                    : null,
                Cor = request.Cor,
                Ativo = request.Ativo
            };

            // 7. Persistir conta no repositório
            await _contaRepository.AdicionarAsync(conta);

            // 8. Retornar resposta
            return new CriarContaResponse
            {
                Id = conta.Id,
                Nome = conta.Nome,
                Tipo = conta.Tipo,
                Saldo = conta.Saldo,
                Limite = request.Limite,
                DiaVencimento = request.DiaVencimento,
                DiaFechamento = diaFechamento,
                Cor = conta.Cor,
                Ativo = conta.Ativo
            };
        }
    }
}
