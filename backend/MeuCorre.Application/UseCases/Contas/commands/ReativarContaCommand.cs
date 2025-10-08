using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Interfaces;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class ReativarContaResponse
    {
        public Guid ContaId { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataAtualizacao { get; set; }
    }

    public class ReativarContaCommand : IRequest<ReativarContaResponse>
    {
        public Guid ContaId { get; set; }
        public Guid UsuarioId { get; set; } 
    }

    public class ReativarContaCommandHandler : IRequestHandler<ReativarContaCommand, ReativarContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public ReativarContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ReativarContaResponse> Handle(ReativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
            {
                throw new ApplicationException($"Conta com ID {request.ContaId} não encontrada ou não pertence ao usuário.");
            }

            conta.Reativar();

            _contaRepository.Atualizar(conta);
            await _contaRepository.SalvarAsync();

            // 4. Retorno
            return new ReativarContaResponse
            {
                ContaId = conta.Id,
                Ativo = conta.Ativo,
                DataAtualizacao = conta.DataAtualizacao ?? DateTime.UtcNow
            };
        }
    }
}