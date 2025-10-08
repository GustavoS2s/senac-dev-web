using System;
using MeuCorre.Domain.Enuns;

namespace MeuCorre.Domain.Entities
{
    public class Conta : Entidade
    {
        public string Nome { get; private set; }
        public TipoConta Tipo { get; private set; }
        public decimal Saldo { get; private set; } = 0;
        public Guid UsuarioId { get; private set; }
        public bool Ativo { get; private set; } = true;

        public decimal? Limite { get; private set; }
        public int? DiaFechamento { get; private set; }
        public int? DiaVencimento { get; private set; }
        public TipoLimite? TipoLimite { get; private set; }
        public string? Cor { get; private set; }
        public string? Icone { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }

        public Usuario Usuario { get; private set; }

        protected Conta() { }

        public Conta(string nome, TipoConta tipo, decimal saldo, Guid usuarioId, decimal? limite = null,
                     int? diaFechamento = null, int? diaVencimento = null,
                     TipoLimite? tipoLimite = null, string? cor = null, string? icone = null)
        {
            this.Nome = nome;
            this.Tipo = tipo;
            this.Saldo = saldo;
            this.UsuarioId = usuarioId;
            this.Limite = limite;
            this.DiaFechamento = diaFechamento;
            this.DiaVencimento = diaVencimento;
            this.TipoLimite = tipoLimite;
            this.Cor = cor;
            this.Icone = icone;
            this.Ativo = true;
        }

        public bool EhCartaoCredito()
        {
            return Tipo == TipoConta.CartaoCredito;
        }

        public bool EhCarteira()
        {
            return Tipo == TipoConta.Carteira;
        }

        public decimal CalcularLimiteDisponivel()
        {
            if (!EhCartaoCredito() || !Limite.HasValue)
            {
                return 0.00m;
            }
            return Limite.Value - Math.Abs(Saldo);
        }

        public bool PodeFazerDebito(decimal valor)
        {
            if (valor <= 0) return false;

            if (EhCartaoCredito())
            {
                return CalcularLimiteDisponivel() >= valor;
            }
            else
            {
                return (Saldo - valor) >= 0;
            }
        }

        public void AtualizarDados(string nome, int? diaFechamento, int? diaVencimento, string? cor, string? icone, decimal? limite, TipoLimite? tipoLimite)
        {
            this.Nome = nome;
            this.DiaFechamento = diaFechamento;
            this.DiaVencimento = diaVencimento;
            this.Cor = cor;
            this.Icone = icone;
            this.Limite = limite;
            this.TipoLimite = tipoLimite;
            this.DataAtualizacao = DateTime.UtcNow;
        }

        public void Inativar()
        {
            this.Ativo = false;
            this.DataAtualizacao = DateTime.UtcNow;
        }

        public void Reativar()
        {
            this.Ativo = true;
            this.DataAtualizacao = DateTime.UtcNow;
        }
    }
}