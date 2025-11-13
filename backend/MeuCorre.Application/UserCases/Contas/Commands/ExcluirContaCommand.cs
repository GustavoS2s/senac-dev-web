using MediatR;
using MeuCorre.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Commands
{
    public class ExcluirContaCommand : IRequest<Unit>
    {
        public required Guid ContaId { get; set; }
        public required Guid UsuarioId { get; set; }

        public required bool Confirmar { get; set; }
    }

    internal class ExcluirContaCommandHandler : IRequestHandler<ExcluirContaCommand, Unit>
    {
        private readonly IContaRepository _contaRepository;

        public ExcluirContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Unit> Handle(ExcluirContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada ou não pertence ao usuário.");
            }

            if (!request.Confirmar)
            {
                throw new InvalidOperationException("Exclusão não confirmada.");
            }

            if (conta.Saldo != 0)
            {
                throw new InvalidOperationException("A conta não pode ser excluída pois o saldo não é zero.");
            }

            await _contaRepository.ExcluirAsync(conta);

            return Unit.Value;
        }
    }
}
