using Microsoft.EntityFrameworkCore;
using MoustafaTasks.Application;
using MoustafaTasks.Domain.Enums;
using System.Linq.Expressions;

namespace MoustafaTasks.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly TestingDbContext _context;
    private readonly DbSet<T> _dbSet;
    public Repository(TestingDbContext context)
    {
        
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<List<T>> GetAll(
        Expression<Func<T, bool>>[]? filters, CancellationToken cancellationToken, params Expression<Func<T, bool>>[]? includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (filters is not null && filters.Length > 0)
        {
            foreach (var filter in filters) { 
                query = query.Where(filter);
            }
        }
        if (includes is not null && includes.Length > 0)
        {
            foreach (var item in includes)
                query = query.Include(item);
        }
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetAll(List<FilterQuery> filters, CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (filters is null || filters.Count == 0)
            return await query.ToListAsync(cancellationToken);
        var parameter= Expression.Parameter(typeof(T), "param");
        Expression? combinedExpression = null;
        foreach (var filter in filters) 
        {
            var property = Expression.Property(parameter, filter.PropertyName);
            var constant = Expression.Constant(filter.Value);
            Expression comparison;
            switch (filter.FilterOperator)
            {
                case FilterOperator.Equals:
                    comparison = Expression.Equal(property, constant); break;
                case FilterOperator.NotEquals:
                    comparison = Expression.NotEqual(property, constant); break;
                case FilterOperator.GreaterThan:
                    comparison = Expression.GreaterThan(property, constant); break;
                case FilterOperator.GreaterThanOrEqual:
                    comparison = Expression.GreaterThanOrEqual(property, constant); break;
                case FilterOperator.LessThan:
                    comparison = Expression.LessThan(property, constant); break;
                case FilterOperator.LessThanOrEqual:
                    comparison= Expression.LessThanOrEqual(property, constant); break;
                case FilterOperator.Contains:
                    comparison = Expression.Call(property, typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!, constant); break;
                case FilterOperator.StartsWith:
                    comparison = Expression.Call(property, typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!, constant); break;
                case FilterOperator.EndsWith:
                    comparison = Expression.Call(property, typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string)})!, constant); break;
                default:
                    throw new NotSupportedException($"Operator {filter.FilterOperator} not supported");

            }
            if (combinedExpression is null)
                combinedExpression = comparison;
            else
            {
                combinedExpression = filter.LogicalOperator == LogicalOperator.And
                    ? Expression.AndAlso(combinedExpression, comparison)
                    : Expression.OrElse(combinedExpression, comparison);
            }
        }
        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter);
        return await query.Where(lambda).ToListAsync();


    }

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.AsQueryable().Where(predicate);
    }
}
