using MoustafaTasks.Domain.Enums;

namespace MoustafaTasks.Application;

public class FilterQuery
{
    public FilterQuery(string propertyName, FilterOperator filterOperator, object? value, LogicalOperator logicalOperator)
    {
        PropertyName = propertyName;
        FilterOperator = filterOperator;
        Value = value;
        LogicalOperator = logicalOperator;
    }

    public string PropertyName { get; set; }
    public FilterOperator FilterOperator { get; set; }
    public object? Value { get; set; }
    public LogicalOperator LogicalOperator { get; set; }
}
