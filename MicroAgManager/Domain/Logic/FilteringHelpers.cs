using System.Linq.Expressions;

namespace Domain.Logic
{
    public static class FilteringHelpers
    {
        public static IQueryable<T> ApplyFilter<T, TValue>(IQueryable<T> query, Expression<Func<T, TValue?>> selector, TValue? filterValue, string filterOperator, TValue? filterEndValue = null)
            where TValue : struct
        {
            if (string.IsNullOrEmpty(filterOperator)) return query;
            if (filterOperator == "between" && filterEndValue == null) return query;
            switch (filterOperator)
            {
                case "eq":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.Equal(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "neq":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.NotEqual(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "gt":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "lt":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.LessThan(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "gte":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "lte":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), selector.Parameters));
                case "between":
                    if (filterEndValue == null) throw new ArgumentException("End value must be provided for 'between' operation");
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.AndAlso(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(filterValue, typeof(TValue?))), Expression.LessThanOrEqual(selector.Body, Expression.Constant(filterEndValue, typeof(TValue?)))), selector.Parameters));
                case "null":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.Equal(selector.Body, Expression.Constant(null, typeof(TValue?))), selector.Parameters));
                case "notnull":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.NotEqual(selector.Body, Expression.Constant(null, typeof(TValue?))), selector.Parameters));
                default:
                    throw new ArgumentException($"Invalid filter operator: {filterOperator}");
            }
        }

    }
}

