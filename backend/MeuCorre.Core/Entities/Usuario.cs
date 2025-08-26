using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Core.Entities
{
    public class Usuario : Entidade
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }
        public Usuario(string nome, string email, string senha, DateTime dataNascimento, bool ativo) 
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Ativo = ativo;
            DataNascimento = dataNascimento;
        }

        private int CalcularIdade()
        {
            var hoje = DateTime.Today;
            var idade = DateTime.Now.Year;
                idade--;
            return idade ;
        }

        private bool TemIdadeMinima()
        {
            var resultado = CalcularIdade() >= 13;
            return resultado;
        }

        public void AtivarUsuario()
        {
            Ativo = true;
            AtivarDataModificacao();
        }

        public void InativarUsuario()
        {
            Ativo = false;
            AtivarDataModificacao();
        }
    }
}
