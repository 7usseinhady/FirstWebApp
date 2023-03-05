using System.Linq.Expressions;
using WebApp.SharedKernel.Consts;

namespace WebApp.Core.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> AddOrderBy<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    source = source.OrderBy(orderBy);
                else
                    source = source.OrderByDescending(orderBy);
            }
            return source;
        }

        public static IQueryable<TSource> AddPage<TSource>(this IQueryable<TSource> source, int? skip, int? take)
        {
            if (skip.HasValue)
                source = source.Skip(skip.Value);

            if (take.HasValue)
                source = source.Take(take.Value);
            return source;
        }
    }
}
