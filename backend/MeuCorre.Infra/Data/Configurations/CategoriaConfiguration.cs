using MeuCorre.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuCorre.Infra.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(c => c.CategoriaId);
            builder.Property(c => c.CategoriaId)
                   .ValueGeneratedNever();
            builder.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Descricao).HasMaxLength(200);
            builder.Property(c => c.Cor).HasMaxLength(50);
            builder.Property(c => c.Icone).HasMaxLength(100);
            builder.Property(c => c.Ativo).HasDefaultValue(true);
        }
    }
}
