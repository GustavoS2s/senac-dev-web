using MediatR;
using MeuCorre.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Commands
{
    public class ReativarContaCommand : IRequest<Unit>
    {
        public required Guid ContaId { get; set; }
        public required Guid UsuarioId { get; set; }
    }

    internal class ReativarContaCommandHandler : IRequestHandler<ReativarContaCommand, Unit>
    {
        private readonly IContaRepository _contaRepository;

        public ReativarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Unit> Handle(ReativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada ou não pertence ao usuário.");
            }

            conta.Ativo = true;

            conta.DataAtualizacao = DateTime.UtcNow;

            await _contaRepository.AtualizarAsync(conta);

            return Unit.Value;
        }
    }
}
