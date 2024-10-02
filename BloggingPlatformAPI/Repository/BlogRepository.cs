using BloggingPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatformAPI.Repository
{
    public class BlogRepository : IRepository<Blog>
    {
        private BlogContext _context;

        public BlogRepository(BlogContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> Get()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog> GetById(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public async Task Add(Blog blog)
        {
            await _context.AddAsync(blog);
        }

        public void Update(Blog blog)
        {
            _context.Blogs.Attach(blog);
            _context.Entry(blog).State = EntityState.Modified;
        }

        public void Delete(Blog blog)
        {
            _context.Blogs.Remove(blog);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        IEnumerable<Blog> Search(Func<Blog, bool> filter) => _context.Blogs.Where(filter).ToList();
    }
}
