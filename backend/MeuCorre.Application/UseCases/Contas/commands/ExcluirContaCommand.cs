using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeuCorre.Domain.Interfaces;

namespace MeuCorre.Application.UseCases.Contas.Commands
{
    public class ExcluirContaResponse
    {
        public bool Sucesso { get; set; } = true;
        public string Mensagem { get; set; } = "Conta excluída permanentemente.";
    }

    public class ExcluirContaCommand : IRequest<ExcluirContaResponse>
    {
        public Guid ContaId { get; set; }
        public Guid UsuarioId { get; set; }
        public bool Confirmar { get; set; }
    }

    public class ExcluirContaCommandHandler : IRequestHandler<ExcluirContaCommand, ExcluirContaResponse>
    {
        private readonly IContaRepository _contaRepository;

        public ExcluirContaCommandHandler(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ExcluirContaResponse> Handle(ExcluirContaCommand request, CancellationToken cancellationToken)
        {
            if (!request.Confirmar)
            {
                throw new Exception("A exclusão permanente deve ser confirmada com o parâmetro 'Confirmar' = true.");
            }

            var conta = await _contaRepository.ObterPorIdEUsuarioAsync(request.ContaId, request.UsuarioId);

            if (conta == null)
            {
                throw new Exception($"Conta com ID {request.ContaId} não encontrada ou não pertence ao usuário.");
            }

            if (Math.Abs(conta.Saldo) > 0)
            {
                throw new Exception("Não é possível excluir a conta. O saldo deve ser zero.");
            }

            _ = _contaRepository.Excluir(conta);
            await _contaRepository.SalvarAsync();

            // 6. Retorno
            return new ExcluirContaResponse();
        }
    }
}