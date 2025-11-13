using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Enums
{
    public enum TipoConta
    {
        Carteira = 1, // Carteira física
        ContaBancaria = 2, // Conta bancária usada para depósitos, saques e transferências
        CartaoCredito = 3 // Cartão de crédito utilizado para compras á prazo
    }
}
