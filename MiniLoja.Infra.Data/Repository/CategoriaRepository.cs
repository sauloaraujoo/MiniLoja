using MiniLoja.Business.Categorias;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Infra.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
