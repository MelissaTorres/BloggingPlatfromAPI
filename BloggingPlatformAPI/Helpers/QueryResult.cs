using Microsoft.EntityFrameworkCore;

namespace BloggingPlatformAPI.Helpers
{
    public class QueryResult
    {
        public QueryResult() { }

        public static IQueryable<T> Pagination<T>(IQueryable<T> list, QueryParameters queryParameters)
        {
            // implement pagination
            var queryableList = list
                .Skip((queryParameters.Page - 1) * queryParameters.Limit)
                .Take(queryParameters.Limit);

            return queryableList;
        }
    }
}
