using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Enuns;
using MeuCorre.Domain.Interfaces;
using System.Text.RegularExpressions;


namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class AtualizarContaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarContaCommand : IRequest<AtualizarContaResponse>
    {
        public Guid ContaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public decimal? Limite { get; set; }
        public int? DiaVencimento { get; set; }
        public string? Cor { get; set; }
        public string? Icone { get; set; }
        public bool Ativo { get; set; } // Necessário para controlar Inativar/Reativar
    }

    public class AtualizarContaCommandHandler : IRequestHandler<AtualizarContaCommand, AtualizarContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public AtualizarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<AtualizarContaResponse> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
        {
            ValidarCommand(request);

            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
            {
                throw new ApplicationException($"Conta com ID {request.ContaId} não encontrada ou não pertence ao usuário.");
            }

            var nomeJaExiste = await _contaRepository.ExisteContaComNomeAsync(request.UsuarioId, request.Nome, request.ContaId);
            if (nomeJaExiste)
            {
                throw new ApplicationException($"Já existe outra conta com o nome '{request.Nome}' para este usuário.");
            }

            // Lógica de Recálculo do DiaFechamento (se for Cartão e Vencimento for alterado)
            int? diaFechamento = conta.DiaFechamento;

            if (conta.EhCartaoCredito() && request.DiaVencimento.HasValue)
            {
                // Aplica a regra de 10 dias antes, como no handler anterior
                diaFechamento = request.DiaVencimento.Value - 10;
                if (diaFechamento <= 0) diaFechamento += 30;
            }

            // 🚨 1. CHAMADA AO MÉTODO DA ENTIDADE (Atualiza os dados editáveis)
            conta.AtualizarDados(
                request.Nome,
                diaFechamento, // Usa o valor calculado/mantido
                request.DiaVencimento,
                request.Cor,
                request.Icone,
                request.Limite,
                null // O command não passa TipoLimite, então passamos null ou um valor da entidade se a regra for complexa
            );

            // 🚨 2. CHAMADA PARA ATIVAR/INATIVAR
            if (request.Ativo && !conta.Ativo)
            {
                conta.Reativar();
            }
            else if (!request.Ativo && conta.Ativo)
            {
                conta.Inativar();
            }
            // OBS: O handler não precisa mais alterar conta.Ativo = request.Ativo;

            _contaRepository.AdicionarAsync(conta);
            await _contaRepository.SalvarAsync();

            return new AtualizarContaResponse
            {
                Id = conta.Id,
                Nome = conta.Nome,
                DataAtualizacao = DateTime.UtcNow // Ou conta.DataAtualizacao, dependendo da sua regra de entidade
            };
        }

        private void ValidarCommand(AtualizarContaCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) || request.Nome.Length < 2 || request.Nome.Length > 50)
            {
                throw new ApplicationException("O nome da conta deve ter entre 2 e 50 caracteres.");
            }

            if (!string.IsNullOrEmpty(request.Cor) && !Regex.IsMatch(request.Cor, "^#([A-Fa-f0-9]{6})$"))
            {
                throw new ApplicationException("A cor deve estar no formato hexadecimal (#RRGGBB).");
            }

            if (request.Limite.HasValue && request.Limite <= 0)
            {
                throw new ApplicationException("O limite, se informado, deve ser positivo.");
            }

            if (request.DiaVencimento.HasValue && (request.DiaVencimento < 1 || request.DiaVencimento > 31))
            {
                throw new ApplicationException("O dia de vencimento, se informado, deve ser entre 1 e 31.");
            }
        }
    }
}