using BloggingPlatformAPI.DTOs;

namespace BloggingPlatformAPI.Helpers
{
    public class BlogQueryParameters : QueryParameters
    {
        public string? Title {  get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static IQueryable<BlogDTO> FilterBlogPosts(IQueryable<BlogDTO> queryableBlogs, BlogQueryParameters blogQueryParameters)
        {
            if (blogQueryParameters == null)
                throw new ArgumentNullException(nameof(blogQueryParameters));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Title))
                queryableBlogs = queryableBlogs.Where(b => b.Title.Contains(blogQueryParameters.Title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Content))
                queryableBlogs = queryableBlogs.Where(b => b.Content.Contains(blogQueryParameters.Content, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Category))
                queryableBlogs = queryableBlogs.Where(b => b.Category.Contains(blogQueryParameters.Category, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(blogQueryParameters.Tags))
                queryableBlogs = queryableBlogs.Where(b => b.Tags.Contains(blogQueryParameters.Tags, StringComparison.OrdinalIgnoreCase));

            if (blogQueryParameters.CreatedAt != null)
                queryableBlogs = queryableBlogs.Where(b => b.CreatedAt >= blogQueryParameters.CreatedAt);

            if (blogQueryParameters.UpdatedAt != null)
                queryableBlogs = queryableBlogs.Where(b => b.UpdatedAt >= blogQueryParameters.UpdatedAt);

            return queryableBlogs;
        }
    }
}
