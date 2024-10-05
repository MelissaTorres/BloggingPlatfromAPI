using System.Linq.Expressions;
using System.Reflection;

namespace BloggingPlatformAPI.Helpers
{
    public class QueryParameters
    {
        private int _maxLimit = 100;
        private int _limit = 50;
        private string _sortOrder = "asc";

        public int Page { get; set; } = 1;
        public int Limit 
        { 
            get 
            { 
                return _limit; 
            }
            set 
            {
                _limit = Math.Min(value, _maxLimit);
            }
        }
        public List<string>? SortBy { get; set; } = null;
        public List<string>? SortOrder { get; set; } = new List<string> { "asc" };   // Default order

        public static IQueryable<T> Pagination<T>(IQueryable<T> list, QueryParameters queryParameters)
        {
            // implement pagination
            var queryableList = list
                .Skip((queryParameters.Page - 1) * queryParameters.Limit)
                .Take(queryParameters.Limit);

            return queryableList;
        }

        public static IQueryable<T> ApplySorting<T>(IQueryable<T> list, QueryParameters sortingParams)
        {
            for (int i = 0; i < sortingParams.SortBy.Count; i++)
            {
                var sortBy = sortingParams.SortBy[i];
                var sortOrder = sortingParams.SortOrder.ElementAtOrDefault(i) ?? "asc";

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null) continue;

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                string orderMethod = (i == 0) ?
                    (sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy") :
                    (sortOrder.ToLower() == "desc" ? "ThenByDescending" : "ThenBy");

                list = list.Provider.CreateQuery<T>(
                    Expression.Call(typeof(Queryable), orderMethod,
                        new Type[] { typeof(T), property.PropertyType }, list.Expression, Expression.Quote(orderByExp))
                );
            }

            return list;
        }
    }
}
