using MiniLoja.Core.Business.Vendedores;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;

namespace MiniLoja.Core.Data.Repository
{
    public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
