using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniLoja.Domain.Entities;

namespace MiniLoja.Infra.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.Property(p => p.Nome)
                .IsRequired();

            builder.Property(p => p.Descricao);

            builder.Property(p => p.Imagem);

            builder.Property(p => p.Preco)
                .HasPrecision(18, 2);

            builder.Property(p => p.QtdEstoque)
                .IsRequired();

            builder.Property(p => p.CategoriaId)
                .IsRequired();

            builder.Property(p => p.DataCriacao)
                .IsRequired();

            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
