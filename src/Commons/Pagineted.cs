using ProvaPub.Models;

namespace ProvaPub.Commons
{
    public static class Pagineted
    {
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            bool hasNext = (page * pageSize) < totalCount;

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                HasNext = hasNext
            };
        }
    }
}
