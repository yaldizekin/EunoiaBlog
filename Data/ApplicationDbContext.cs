using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Areas.Admin.Models;

namespace Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog.Areas.Admin.Models.EunBlog>? Blogs { get; set; }
        public DbSet<Blog.Areas.Admin.Models.Contact>? Contact { get; set; }
    }
}