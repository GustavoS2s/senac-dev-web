using MeuCorre.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.infra.Data.Configurations
{
    class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("Contas");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nome).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Tipo).IsRequired();
            builder.Property(c => c.Saldo).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(c => c.UsuarioId).IsRequired();
            builder.Property(c => c.Ativo).IsRequired();
            builder.Property(c => c.DataCriacao).IsRequired();
            builder.Property(c => c.Limite).IsRequired(false).HasColumnType("decimal(10,2)");
            builder.Property(c => c.DiaFechamento).IsRequired(false);
            builder.Property(c => c.DiaVencimento).IsRequired(false);
            builder.Property(c => c.Cor).IsRequired(false).HasMaxLength(7);
            builder.Property(c => c.Icone).IsRequired(false).HasMaxLength(20);
            builder.Property(c => c.DataAtualizacao).IsRequired(false);

            builder.HasOne(c => c.Usuario)
               .WithMany()
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
