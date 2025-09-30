using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeuCorre.Domain.Entities;
using MeuCorre.Domain.Enuns; 

namespace MeuCorre.Infrastructure.Data.Configurations
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("Contas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(50); 

            builder.Property(c => c.Tipo)
                .HasConversion<int>() 
                .IsRequired();

            builder.Property(c => c.Saldo)
                .IsRequired()
                .HasColumnType("decimal(10,2)"); 

            builder.Property(c => c.Ativo)
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .IsRequired(false);


            builder.Property(c => c.Limite)
                .IsRequired(false) 
                .HasColumnType("decimal(10,2)"); 

            builder.Property(c => c.DiaFechamento)
                .IsRequired(false);

            builder.Property(c => c.DiaVencimento)
                .IsRequired(false);

            builder.Property(c => c.TipoLimite)
                .HasConversion<int>() 
                .IsRequired(false);

            builder.Property(c => c.Cor)
                .IsRequired(false)
                .HasMaxLength(7); 

            builder.Property(c => c.Icone)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.HasOne(c => c.Usuario)
                .WithMany() 
                .HasForeignKey(c => c.UsuarioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasIndex(c => c.UsuarioId);
            builder.HasIndex(c => c.Tipo);
            builder.HasIndex(c => c.Ativo);

            builder.HasIndex(c => new { c.UsuarioId, c.Nome })
                   .IsUnique();
        }
    }
}