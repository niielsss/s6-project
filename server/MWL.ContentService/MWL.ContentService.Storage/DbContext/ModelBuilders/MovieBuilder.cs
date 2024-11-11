using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Storage.DbContext.ModelBuilders
{
    public class MovieBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Movie>();

            model.ToTable("Movies");

            model.HasIndex(x => x.Id)
                .IsUnique();

            model.Property(x => x.Id)
                .IsRequired();

            model.Property(x => x.Title)
                .IsRequired();

            model.Property(x => x.PlotSummary)
                .IsRequired();

            model.Property(x => x.ReleaseDate)
                .IsRequired();

            model.Property(x => x.GenreId)
                .IsRequired();

            model.Property(x => x.DirectorId)
                .IsRequired(false);

            model.HasOne(x => x.Director)
                .WithMany(x => x.DirectedMovies)
                .HasForeignKey(x => x.DirectorId);

            model.HasMany(x => x.Cast)
                .WithMany(x => x.MoviesAsCast);

            model.Property(x => x.Duration)
                .IsRequired();

            model.Property(x => x.ProductionCompany)
                .IsRequired();

            model.Property(x => x.CreatedAt)
                .IsRequired();

            model.Property(x => x.UpdatedAt)
                .IsRequired();

            model.Ignore(x => x.Reviews);
        }
    }
}
