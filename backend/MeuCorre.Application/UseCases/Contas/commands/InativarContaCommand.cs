using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Interfaces;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class InativarContaResponse
    {
        public Guid ContaId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class InativarContaCommand : IRequest<InativarContaResponse>
    {
        public Guid ContaId { get; set; }
        public Guid UsuarioId { get; set; } 
    }

    public class InativarContaCommandHandler : IRequestHandler<InativarContaCommand, InativarContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public InativarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<InativarContaResponse> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
            {
                throw new ApplicationException($"Conta com ID {request.ContaId} não encontrada ou não pertence ao usuário.");
            }

            if (Math.Abs(conta.Saldo) > 0)
            {
                throw new ApplicationException("A conta só pode ser inativada se o saldo for zero.");
            }

            conta.Inativar();

            _contaRepository.Atualizar(conta);
            await _contaRepository.SalvarAsync();

            return new InativarContaResponse
            {
                ContaId = conta.Id,
                Ativo = conta.Ativo,
                DataAtualizacao = conta.DataAtualizacao ?? DateTime.UtcNow
            };
        }
    }
}