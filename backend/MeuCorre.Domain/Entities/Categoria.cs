
using MeuCorre.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Entities
{
    public class Categoria : Entidade
    {
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public Tipotransacao Tipo  { get; private set; }
        public string? Cor { get; private set; }
        public string? Icone { get; private set; }
        public Guid? UsuarioId { get; private set; }
        public bool Ativo { get; private set; }
        public Categoria() { }

        public virtual Usuario Usuario { get; private set; }
        public Categoria(string nome,Tipotransacao tipo, bool ativo, string? descricao, string? cor, string? icone, Guid? usuarioid)
        {
            ValidarEntidadeCategoria(cor);
            Nome = nome;
            Descricao = descricao;
            Cor = cor;
            Icone = icone;
            UsuarioId = usuarioid;
            Ativo = true;
            Tipo = tipo;
        }

        public void AtualizarInformacoes(string nome,Tipotransacao tipo, bool ativo, string? descricao, string? cor, string? icone)
        {
            Nome = nome.ToUpper();
            Descricao = descricao;
            Cor = cor;
            Icone = icone;
            Tipo = tipo;
            Ativo = ativo;
            AtualizarDataModificacao();
        }
        public void Ativar()
        {
            Ativo = true;
            AtualizarDataModificacao();
        }

        public void Inativar()
        {
            Ativo = false;
            AtualizarDataModificacao();
        }

        private void ValidarEntidadeCategoria(string cor)
        {
            if(string.IsNullOrEmpty(cor))
            {
                return;
            }

            var corRegex = new Regex(@"^#?([0-9a-fA-F]{3}){1,2}$");

            if (!corRegex.IsMatch(cor))
            {
                throw new Exception("A cor deve estar no formato hexadecimal.");
            }
        }
    }
}
