using MoustafaTasks.Application;
using MoustafaTasks.Domain.Enums;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text.Json;

namespace MoustafaTasks.Infrastructure;

public class FilteringService : IFilteringService
{
    private static object? ConvertJsonValue(JsonElement element, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
        try
        {
            if (underlying.IsEnum)
            {
                var stringValue = element.ValueKind == JsonValueKind.String
                    ? element.GetString()
                    : element.GetRawText();
                return Enum.Parse(underlying, stringValue!, ignoreCase: true);
            }
            if (underlying == typeof(bool))
            {
                if (element.ValueKind == JsonValueKind.True) return true;
                if (element.ValueKind == JsonValueKind.False) return false;
                var boolString = element.GetString();
                return bool.TryParse(boolString, out var result) && result;
            }
            if (underlying == typeof(int)) return element.GetInt32();
            if (underlying == typeof(long)) return element.GetInt64();
            if (underlying == typeof(decimal)) return element.GetDecimal();
            if (underlying == typeof(double)) return element.GetDouble();
            if (underlying == typeof(DateTime)) return element.GetDateTime();
            if (underlying == typeof(Guid)) return element.GetGuid();

            var raw = element.GetString();
            return Convert.ChangeType(raw, underlying);

        }
        catch (Exception ex) { throw new InvalidOperationException($"Failed to convert value '{element}' to '{underlying}'"); }
        
    }
    private static Expression BuildPropertyExpression(ParameterExpression parameter, string propertyPath)
    {
        var expr = (Expression)parameter;
        foreach (var member in propertyPath.Split('.')) { expr = Expression.PropertyOrField(expr, member); }
        return expr;
    }
    private static Expression BuildComparison(Expression left, FilterQuery filterQuery) 
    {
        var leftType = left.Type;
        var underlying = Nullable.GetUnderlyingType(leftType) ?? leftType;
        object? converted = filterQuery.Value;
        if (filterQuery.Value is not null && !underlying.IsAssignableFrom(filterQuery.Value.GetType())) 
        {
            if (filterQuery.Value is JsonElement jsonElement)
            {
                //if (underlying.IsEnum)
                //{
                //    if (jsonElement.ValueKind == JsonValueKind.String)
                //        converted = Enum.Parse(underlying, jsonElement.GetString()!);
                //    else if (jsonElement.ValueKind == JsonValueKind.Number)
                //    {
                //        var numberValue = jsonElement.GetInt32();
                //        converted = Enum.ToObject(underlying, numberValue);
                //    }
                //}
                //else
                // converted = JsonSerializer.Deserialize(jsonElement.GetRawText(), underlying);
                converted = ConvertJsonValue(jsonElement, underlying);
            }
            else
            converted = Convert.ChangeType(filterQuery.Value, underlying);
        }
        var constant = Expression.Constant(converted, underlying);
        if(leftType != underlying)
            left = Expression.Convert(left, underlying);
        if (underlying == typeof(string)) { return BuildStringComparison(left, constant, filterQuery); }
        if (filterQuery.FilterOperator == Domain.Enums.FilterOperator.NotEquals)
        {
            var nullConst = Expression.Constant(null, leftType);
            return Expression.NotEqual(Expression.Convert(left, leftType), nullConst);
        }
        return filterQuery.FilterOperator switch { 
          FilterOperator.Equals => Expression.Equal(left, constant),
          FilterOperator.NotEquals => Expression.NotEqual(left, constant),
          FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, constant),
          FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, constant),
          FilterOperator.GreaterThan => Expression.GreaterThan(left, constant),
          FilterOperator.LessThan => Expression.LessThan(left, constant),
          _ => throw new NotSupportedException($"Operator {filterQuery.FilterOperator} not supported")

        };
    }
    private static Expression BuildStringComparison(Expression left, Expression right, FilterQuery filterQuery)
    {
        var leftNotNull = Expression.NotEqual(Expression.Convert(left, typeof(string)),Expression.Constant(null, typeof(string)));
        var leftNonNull = Expression.Condition(leftNotNull, Expression.Convert(left, typeof(string)), Expression.Constant(string.Empty));
        right = Expression.Convert(right, typeof(string));
        return filterQuery.FilterOperator switch
        {
            FilterOperator.Equals => Expression.Equal(leftNonNull, right),
            FilterOperator.NotEquals => Expression.NotEqual(leftNonNull, right),
            FilterOperator.Contains => Expression.Call(leftNonNull, nameof(string.Contains), Type.EmptyTypes, right),
            FilterOperator.StartsWith => Expression.Call(leftNonNull, nameof(string.StartsWith), Type.EmptyTypes, right),
            FilterOperator.EndsWith => Expression.Call(leftNonNull, nameof(string.EndsWith), Type.EmptyTypes, right),
            _ => throw new NotSupportedException($"Operator {filterQuery.FilterOperator} not supported")

        };
    }
    private static Expression BuildIn(Expression left, object? value, bool negate, Type targetType)
    {
        if (value is not System.Collections.IEnumerable enumerable || value is string)
        { throw new ArgumentException("In/NotIn operator requires a non-string IEnumberable value"); }
        var listType = typeof(List<>).MakeGenericType(targetType);
        var list = (System.Collections.IList)Activator.CreateInstance(listType)!;

        foreach (var item in enumerable)
        {
            var v = item;
            if(v is not null && !targetType.IsAssignableFrom(v.GetType()))
                v = Convert.ChangeType(v, targetType);
            list.Add(v);
        }
        var listConst = Expression.Constant(list, listType);
        var contains = listType.GetMethod("Contains", new[] { targetType })!;
        var call = Expression.Call(listConst, contains, left);
        return negate ? Expression.Not(call) : call;
    }
    public Expression<Func<T, bool>> BuildPredicate<T>(IReadOnlyList<FilterQuery> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        Expression? combined = null;
        foreach (var filter in filters)
        {
            var propertyExpression = BuildPropertyExpression(parameter, filter.PropertyName);
            var comparison = BuildComparison(propertyExpression, filter);
            combined = combined is null
                ? comparison
                : filter.LogicalOperator == LogicalOperator.And 
                    ? Expression.AndAlso(combined, comparison) 
                    : Expression.OrElse(combined, comparison);
        }
        if (combined is null)
            combined = Expression.Constant(true);
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
}
