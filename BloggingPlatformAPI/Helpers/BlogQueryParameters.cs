using BloggingPlatformAPI.DTOs;

namespace BloggingPlatformAPI.Helpers
{
    public class BlogQueryParameters : QueryParameters
    {
        public string? Title {  get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string? Tags { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public static IQueryable<BlogDTO> FilterBlogPosts(IQueryable<BlogDTO> queryableBlogs, BlogQueryParameters blogQueryParameters)
        {
            if (queryableBlogs == null || blogQueryParameters == null)
                throw new ArgumentNullException(nameof(blogQueryParameters));

            var blogs = queryableBlogs.Provider.CreateQuery<BlogDTO>(queryableBlogs.Expression);

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Title))
                blogs = blogs.Where(b => b.Title.Contains(blogQueryParameters.Title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Content))
                blogs = blogs.Where(b => b.Content.Contains(blogQueryParameters.Content, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Category))
                blogs = blogs.Where(b => b.Category.Contains(blogQueryParameters.Category, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Tags))
                blogs = blogs.Where(b => b.Tags.Contains(blogQueryParameters.Tags, StringComparison.OrdinalIgnoreCase));

            if (blogQueryParameters.CreatedAt != null)
                blogs = blogs.Where(b => b.CreatedAt >= blogQueryParameters.CreatedAt);

            if (blogQueryParameters.UpdatedAt != null)
                blogs = blogs.Where(b => b.UpdatedAt >= blogQueryParameters.UpdatedAt);

            return blogs;
        }

        //public static IQueryable<BlogDTO> SortBlogPosts(IQueryable<BlogDTO> queryableBlogs, BlogQueryParameters blogQueryParameters)
        //{
        //    if (queryableBlogs == null || blogQueryParameters == null)
        //        throw new ArgumentNullException(nameof(blogQueryParameters));

        //    var blogs = Utils.DeepClone(queryableBlogs);

        //    if (blogQueryParameters.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
        //        blogs = blogQueryParameters.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
        //            ? blogs.OrderByDescending(b => b.Title) : blogs.OrderBy(b => b.Title);

        //    if (blogQueryParameters.SortBy.Equals("Content", StringComparison.OrdinalIgnoreCase))
        //        blogs = blogQueryParameters.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
        //            ? blogs.OrderByDescending(b => b.Content) : blogs.OrderBy(b => b.Content);

        //    if (blogQueryParameters.SortBy.Equals("Content", StringComparison.OrdinalIgnoreCase))
        //        blogs = blogQueryParameters.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
        //            ? blogs.OrderByDescending(b => b.Content) : blogs.OrderBy(b => b.Content);

        //    return null;
        //}
    }
}
