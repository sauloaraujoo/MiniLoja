using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniLoja.Core.Domain.Entities;

namespace MiniLoja.Core.Data.Mappings
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.Property(c => c.Nome)
            .IsRequired();

            builder.Property(c => c.Descricao);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
