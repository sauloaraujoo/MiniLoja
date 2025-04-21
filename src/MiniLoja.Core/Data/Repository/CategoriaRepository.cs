using MiniLoja.Core.Business.Categorias;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;

namespace MiniLoja.Core.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
