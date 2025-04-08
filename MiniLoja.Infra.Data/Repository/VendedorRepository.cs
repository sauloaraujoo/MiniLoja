using MiniLoja.Business.Vendedores;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Infra.Data.Repository
{
    public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(MiniLojaContext db) : base(db)
        {
        }
    }
}
