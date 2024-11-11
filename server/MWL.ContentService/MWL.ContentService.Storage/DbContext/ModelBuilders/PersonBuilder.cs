using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Storage.DbContext.ModelBuilders
{
    public class PersonBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Person>();

            model.ToTable("People");

            model.HasIndex(x => x.Id)
                .IsUnique();

            model.Property(x => x.Id)
                .IsRequired();

            model.Property(x => x.Name)
                .IsRequired();

            model.HasMany(x => x.DirectedMovies)
                .WithOne(x => x.Director);

            model.HasMany(x => x.MoviesAsCast)
                .WithMany(x => x.Cast);

            model.Property(x => x.CreatedAt)
                .IsRequired();

            model.Property(x => x.UpdatedAt)
                .IsRequired();
        }
    }
}
