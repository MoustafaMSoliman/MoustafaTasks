using MoustafaTasks.Domain;

namespace MoustafaTasks.Application;

public class GenericFilterService<T> : IGenericFilterService<T> where T :class
{
    private IRepository<T> _repository;
    private readonly IFilteringService _filtering;


    public GenericFilterService(IRepository<T> repository, IFilteringService filtering)
    {
        _repository = repository;
        _filtering = filtering;

    }

    public async Task<List<T>> GetAll(List<FilterQuery> filters, CancellationToken cancellationToken)
    {
        if (filters.Count > 0)
        {
            var predicate = _filtering.BuildPredicate<T>(filters);
            return await Task.FromResult(_repository.Where(predicate).ToList());

        }
        return await _repository.GetAll(filters, cancellationToken);
    }

    
}
