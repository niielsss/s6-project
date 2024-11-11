﻿// <auto-generated />
using System;
using MWL.ContentService.Storage.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MWL.ContentService.Api.Migrations
{
    [DbContext(typeof(ContentContext))]
    partial class ContentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MWL.ContentService.Domain.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DirectorId")
                        .HasColumnType("integer");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<int>("Genre")
                        .HasColumnType("integer");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<string>("PlotSummary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductionCompany")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Movies", (string)null);
                });

            modelBuilder.Entity("MWL.ContentService.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("MoviePerson", b =>
                {
                    b.Property<int>("CastId")
                        .HasColumnType("integer");

                    b.Property<int>("MoviesAsCastId")
                        .HasColumnType("integer");

                    b.HasKey("CastId", "MoviesAsCastId");

                    b.HasIndex("MoviesAsCastId");

                    b.ToTable("MoviePerson");
                });

            modelBuilder.Entity("MWL.ContentService.Domain.Entities.Movie", b =>
                {
                    b.HasOne("MWL.ContentService.Domain.Entities.Person", "Director")
                        .WithMany("DirectedMovies")
                        .HasForeignKey("DirectorId");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("MoviePerson", b =>
                {
                    b.HasOne("MWL.ContentService.Domain.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("CastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MWL.ContentService.Domain.Entities.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesAsCastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MWL.ContentService.Domain.Entities.Person", b =>
                {
                    b.Navigation("DirectedMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
