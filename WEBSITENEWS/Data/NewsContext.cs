using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBSITENEWS.Models
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Teammember> Teammember { get; set; }
  
        public DbSet<ContactUs> Contacts { get; set; }


    }
}
