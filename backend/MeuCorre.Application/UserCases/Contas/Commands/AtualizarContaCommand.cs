using MediatR;
using MeuCorre.Application.Interfaces;
using MeuCorre.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuCorre.Application.UserCases.Contas.Commands
{
    public class AtualizarContaCommand : IRequest<Unit>
    {
        public required Guid ContaId { get; set; }
        public required Guid UsuarioId { get; set; }
        public string? Nome { get; set; }
    }

    internal class AtualizarContaCommandHandler : IRequestHandler<AtualizarContaCommand, Unit>
    {
        private readonly IContaRepository _contaRepository;

        public AtualizarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Unit> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada.");
            }

            if (request.Nome != null && string.IsNullOrWhiteSpace(request.Nome))
            {
                throw new ArgumentException("Nome não pode ser vazio.");
            }

            if (request.Nome != null)
                conta.Nome = request.Nome;

            conta.DataAtualizacao = DateTime.UtcNow;

            await _contaRepository.AtualizarAsync(conta);

            return Unit.Value;
        }
    }
}
