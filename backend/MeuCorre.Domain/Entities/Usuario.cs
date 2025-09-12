using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Entities
{
    public class Usuario : Entidade
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }

        public virtual ICollection<Categoria> Categorias { get; private set; }
        public Usuario(string nome, string email, string senha, DateTime dataNascimento, bool ativo) 
        {
            Nome = nome;
            Email = email;
            Senha = ValidarSenha(senha);
            Ativo = ativo;
            DataNascimento = ValidarIdadeMininma(dataNascimento);
        }

        private DateTime ValidarIdadeMininma(DateTime nascimento)
        {
            var hoje = DateTime.Today;
            var idade = DateTime.Now.Year;
            idade--;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;

            if (idade < 13)
            {
                throw new Exception("Usuário deve ser maior de 13 anos.");
            }
            return nascimento;
        }
        
        public string ValidarSenha(string senha)
        {
            if (!Regex.IsMatch(senha, "[a-z]"))
            {
                throw new Exception("Senha deve conter pelo menos uma letra minuscula.");
            }

            if (!Regex.IsMatch(senha, "[A-Z]"))
            {
                throw new Exception("Senha deve conter pelo menos uma letra maiuscula.");
            }

            if (!Regex.IsMatch(senha, "[0-9]"))
            {
                throw new Exception("Senha deve conter pelo menos um número");
            }
            return senha;


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
