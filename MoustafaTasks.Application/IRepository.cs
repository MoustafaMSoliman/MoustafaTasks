using System.Linq.Expressions;

namespace MoustafaTasks.Application;

public interface IRepository<T> where T : class
{
    IQueryable<T> AsQueryable();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAll(Expression<Func<T, bool>>[]? filters, CancellationToken cancellationToken, params Expression<Func<T, bool>>[]? includes);
    Task<List<T>> GetAll(List<FilterQuery> filters, CancellationToken cancellationToken);
}
