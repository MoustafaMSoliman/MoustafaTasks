using System.Linq.Expressions;

namespace MoustafaTasks.Application;

public interface IFilteringService
{
    Expression<Func<T, bool>> BuildPredicate<T>(IReadOnlyList<FilterQuery> filters);
}
