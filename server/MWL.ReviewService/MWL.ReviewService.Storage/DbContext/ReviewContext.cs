using Microsoft.EntityFrameworkCore;
using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Storage.DbContext.ModelBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Storage.DbContext
{
    public class ReviewContext(DbContextOptions<ReviewContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ReviewBuilder.Build(modelBuilder);
        }
    }
}
