using MediatR;
using MeuCorre.Application.Interfaces;
using MeuCorre.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Commands
{
    public class InativarContaCommand : IRequest<Unit>
    {
        public required Guid ContaId { get; set; }
        public required Guid UsuarioId { get; set; }
    }

    internal class InativarContaCommandHandler : IRequestHandler<InativarContaCommand, Unit>
    {
        private readonly IContaRepository _contaRepository;

        public InativarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Unit> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada ou não pertence ao usuário.");
            }

            if (conta.Saldo != 0)
            {
                throw new InvalidOperationException("Não é possível inativar a conta porque o saldo não é zero.");
            }

            conta.Ativo = false;

            conta.DataAtualizacao = DateTime.UtcNow;

            await _contaRepository.AtualizarAsync(conta);

            return Unit.Value;
        }
    }
}
