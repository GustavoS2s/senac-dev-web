
using MeuCorre.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Domain.Entities
{
    public class Categoria : Entidade
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public Tipotransacao Tipo  { get; private set; }
        public string Cor { get; private set; }
        public string Icone { get; private set; }
        public Guid? UsuarioId { get; private set; }
        public bool Ativo { get; private set; }

        public virtual Usuario Usuario { get; private set; }
        public Categoria(string nome, string descricao, string cor, string icone, Guid usuarioid, bool ativo)
        {
            Nome = nome;
            Descricao = descricao;
            Cor = cor;
            Icone = icone;
            UsuarioId = usuarioid;
            Ativo = ativo;
        }
    }
}
