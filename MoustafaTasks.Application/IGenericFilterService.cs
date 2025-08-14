using MoustafaTasks.Domain;

namespace MoustafaTasks.Application;

public interface IGenericFilterService<T> where T : class
{
    Task<List<T>> GetAll(List<FilterQuery> filters, CancellationToken cancellationToken);

}
