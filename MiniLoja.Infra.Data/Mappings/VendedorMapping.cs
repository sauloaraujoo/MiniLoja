using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniLoja.Domain.Entities;

namespace MiniLoja.Infra.Data.Mappings
{
    public class VendedorMapping : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedores");

            builder.HasOne(v => v.AspnetUser)
                .WithOne() 
                .HasForeignKey<Vendedor>(v => v.AspnetUserId)
                .IsRequired();
        }
    }
}
