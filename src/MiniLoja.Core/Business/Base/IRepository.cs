using MiniLoja.Core.Domain.Entities;
using System.Linq.Expressions;

namespace MiniLoja.Core.Business.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task AdicionarAsync(TEntity entity);
        Task<TEntity> ObterPorId(int id);
        Task<List<TEntity>> ObterTodos();
        Task AtualizarAsync(TEntity entity);
        Task RemoverAsync(int id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
