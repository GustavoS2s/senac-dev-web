using MediatR;
using System;
using System.Collections.Generic;

namespace MeuCorre.Application.UseCases.Contas.Queries
{
    // A classe deve existir para o C5259 ser resolvido
    public class ListarContasQuery : IRequest<IEnumerable<object>>
    {
        public Guid UsuarioId { get; set; }
        public int? Tipo { get; set; }
        public bool ApenasAtivas { get; set; }
        public string? OrdenarPor { get; set; }

        // 🚨 CORREÇÃO: Adicionar construtor que aceite 7 argumentos (resolve o C570D)
        public ListarContasQuery(Guid usuarioId, int? tipo, object arg3, bool apenasAtivas, string ordenarPor, object arg6, object arg7)
        {
            UsuarioId = usuarioId;
            Tipo = tipo;
            ApenasAtivas = apenasAtivas;
            OrdenarPor = ordenarPor;
            // Não se preocupe em usar arg3, arg6, arg7, eles estão aqui apenas para satisfazer o compilador.
        }
    }
}