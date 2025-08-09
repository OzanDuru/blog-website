using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSitem.Identity;// BlogIdentityUser ve BlogIdentityRole'un bulunduğu namespace

namespace WebSitem.Identity // veya projenize uygun başka bir namespace
{
    // Bu context SADECE Identity tablolarını yönetir
    public class BlogIdentityDbContext : IdentityDbContext<BlogIdentityUser, BlogIdentityRole, string>
    {
        public BlogIdentityDbContext(DbContextOptions<BlogIdentityDbContext> options)
            : base(options)
        {

        }


    }
}


/* Bu sınıf, IdentityDbContext'ten türediği için Identity tablolarını yönetmekle görevlidir. İçine uygulamanızın diğer 
 DbSet'lerini (Bloglar, Kategoriler vb.) koymayacaksınız.*/