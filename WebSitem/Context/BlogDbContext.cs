using System;
using Microsoft.EntityFrameworkCore;
using WebSitem.Models;

namespace WebSitem.Context 
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FileResource> FileResources { get; set; }
        public DbSet<FollowedBlog> FollowedBlogs { get; set; }

        
    }
}