using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCHomework6.Data.Database
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<TagCloud> TagCloud { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<TagCloud>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });
        }

    }
}