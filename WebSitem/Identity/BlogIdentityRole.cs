using System;
using Microsoft.AspNetCore.Identity;
namespace WebSitem.Identity
{
    public class BlogIdentityRole : IdentityRole
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }

        public BlogIdentityRole() : base() { }

        public BlogIdentityRole(string roleName) : base(roleName) { }
    }
}