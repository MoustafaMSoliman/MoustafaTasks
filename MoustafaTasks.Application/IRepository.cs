using System.Linq.Expressions;

namespace MoustafaTasks.Application;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAll(Expression<Func<T, bool>>[]? filters, CancellationToken cancellationToken, params Expression<Func<T, bool>>[]? includes);
    Task<List<T>> GetAll(List<FilterQuery> filters, CancellationToken cancellationToken);
}
