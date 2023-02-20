using System.Linq.Expressions;

namespace FlowerSpot.SharedKernel.Contracts;
public interface IBaseRepository<T>
{
    Task<IReadOnlyList<T>> GetAll();
    Task<T?> GetById(int id);
    Task<T?> GetSingleMatch(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyCollection<T>> GetMultipleMatches(Expression<Func<T, bool>> predicate);
    Task Update(T entity);
    Task<T> Add(T entity);
    Task AddMultiple(List<T> entities);
    Task Delete(T entity);
}
