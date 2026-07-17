using System.Data;
using Dapper;

namespace Portfolio.Persistence.Common;

/// <summary>
/// Dapper type handler so <see cref="DateOnly"/> values can be passed as SQL
/// parameters (bound as <c>DbType.Date</c>) and read back from DATE columns.
/// Without this, Dapper's DynamicParameters throws
/// "The member ... of type System.DateOnly cannot be used as a parameter value".
/// The same handler also covers <see cref="Nullable{DateOnly}"/>.
/// </summary>
public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value) => value switch
    {
        DateTime dateTime => DateOnly.FromDateTime(dateTime),
        DateOnly dateOnly => dateOnly,
        string text => DateOnly.Parse(text),
        _ => DateOnly.FromDateTime(Convert.ToDateTime(value))
    };
}

/// <summary>
/// Dapper type handler so <see cref="TimeOnly"/> values can be passed as SQL
/// parameters (bound as <c>DbType.Time</c>) and read back from TIME columns.
/// </summary>
public sealed class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.ToTimeSpan();
    }

    public override TimeOnly Parse(object value) => value switch
    {
        TimeSpan timeSpan => TimeOnly.FromTimeSpan(timeSpan),
        DateTime dateTime => TimeOnly.FromDateTime(dateTime),
        TimeOnly timeOnly => timeOnly,
        string text => TimeOnly.Parse(text),
        _ => TimeOnly.FromTimeSpan((TimeSpan)value)
    };
}
