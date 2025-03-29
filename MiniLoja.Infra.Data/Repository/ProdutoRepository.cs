using MiniLoja.Business.Produtos;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Infra.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
