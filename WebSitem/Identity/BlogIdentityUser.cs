using System;
using Microsoft.AspNetCore.Identity;

namespace WebSitem.Identity
{
    public class BlogIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}