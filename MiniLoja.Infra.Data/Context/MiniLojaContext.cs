using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Domain.Entities;

namespace MiniLoja.Infra.Data.Context
{
    public class MiniLojaContext : IdentityDbContext
    {
        public MiniLojaContext(DbContextOptions<MiniLojaContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

    }
}
