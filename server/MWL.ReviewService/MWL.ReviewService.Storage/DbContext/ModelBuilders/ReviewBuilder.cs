using Microsoft.EntityFrameworkCore;
using MWL.ReviewService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Storage.DbContext.ModelBuilders
{
    public class ReviewBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Review>();

            model.ToTable("Reviews");

            model.HasKey(x => x.Id);

            model.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            model.Property(x => x.MovieId)
                .HasColumnName("MovieId")
                .IsRequired();

            model.Property(x => x.UserId)
                .HasColumnName("UserId")
                .IsRequired();
        }
    }
}
