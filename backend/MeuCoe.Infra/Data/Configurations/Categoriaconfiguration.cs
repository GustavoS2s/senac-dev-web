using MeuCorre.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;

namespace MeuCorre.Infra.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(categoria => categoria.Id);
            builder.Property(categoria => categoria.Nome).IsRequired().HasMaxLength(100);
            builder.Property(categoria => categoria.Ativo).IsRequired();
            builder.Property(categoria => categoria.DataCriacao).IsRequired();
            builder.Property(categoria => categoria.DataAtualizacao).IsRequired(false);
            builder.Property(categoria => categoria.Descricao).HasMaxLength(200);
            builder.Property(categoria => categoria.Cor).HasMaxLength(50);
            builder.Property(categoria => categoria.Icone).HasMaxLength(100);
            builder.Property(categoria => categoria.UsuarioId).IsRequired(false);
            builder.Property(categoria => categoria.Tipo).IsRequired();

           builder.HasOne(categoria => categoria.Usuario)
                   .WithMany(usuario => usuario.Categorias)
                   .HasForeignKey(categoria => categoria.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
