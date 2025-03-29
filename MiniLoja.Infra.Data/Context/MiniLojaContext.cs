using Microsoft.EntityFrameworkCore;
using MiniLoja.Domain.Entities;

namespace MiniLoja.Infra.Data.Context
{
    public class MiniLojaContext : DbContext
    {
        public MiniLojaContext(DbContextOptions<MiniLojaContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        
    }
}
