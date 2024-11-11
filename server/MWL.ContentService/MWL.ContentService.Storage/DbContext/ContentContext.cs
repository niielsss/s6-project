using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Storage.DbContext.ModelBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Storage.DbContext
{
    public class ContentContext(DbContextOptions<ContentContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MovieBuilder.Build(modelBuilder);
            PersonBuilder.Build(modelBuilder);
        }
    }
}
