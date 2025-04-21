using MiniLoja.Core.Business.Produtos;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;

namespace MiniLoja.Core.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
