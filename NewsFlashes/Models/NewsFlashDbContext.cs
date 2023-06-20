using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewsFlashes.Models
{
    public class NewsFlashDbContext : DbContext
    {
        public NewsFlashDbContext() : base("name=MySQLConnection")
        {
            Database.SetInitializer<NewsFlashDbContext>(null);
        }
        public DbSet<NewsFlash> NewsFlashes { get; set; }
    }
}