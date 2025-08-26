using MeuCorre.Core.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Core.Entities
{
    public class Categoria : Entidade
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public Tipotransacao tipo  { get; private set; }
        public string Cor { get; private set; }
        public string Icone { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativo { get; private set; }
        public Categoria(string nome, string descricao, string cor, string icone, DateTime datacriacao, bool ativo)
        {
            Nome = nome;
            Descricao = descricao;
            Cor = cor;
            Icone = icone;
            Ativo = ativo;
            DataCriacao = datacriacao;
        }
    }
}
