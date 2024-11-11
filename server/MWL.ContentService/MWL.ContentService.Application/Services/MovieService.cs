using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Entities.Extensions;
using MWL.ContentService.Domain.Exceptions;
using MWL.ContentService.Domain.Models;
using MWL.ContentService.Domain.Repositories;
using MWL.ContentService.Domain.Services;
using MWL.ContentService.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Application.Services;

public class MovieService(IMovieRepository movieRepository) : IMovieService
{
    public async Task<int> CreateAsync(Movie movie, CancellationToken cancellationToken)
    {
        if (movie is null)
            throw new ValidationError("Movie is null.");

        // TODO: Validate probably
        movie.ReleaseDate = movie.ReleaseDate.ToUniversalTime();

        movie.SetCreatedDate();
        movie.SetUpdatedDate();

        movieRepository.Add(movie);
        await movieRepository.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new ValidationError("Id is invalid.");

        var entity = await movieRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundError(id);

        movieRepository.Delete(entity);
        await movieRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Movie> GetByFilterAsync(MovieFilter filter, CancellationToken cancellationToken)
    {
        if (filter is null)
            throw new ValidationError("Filter is null.");

        var movie = await movieRepository.GetByFilterAsync(filter, cancellationToken);

        var reviewsMessaging = new ReviewsMessaging();

        Console.WriteLine($"Calling ReviewsMessaging for movie: {movie.Id}");

        string reviews;

        try
        {
            var brokerMessage = new BrokerMessage { Action = "GetReviews", Data = movie.Id.ToString() };
            reviews = await reviewsMessaging.CallAsync(brokerMessage, cancellationToken);
            var reviewsList = JsonConvert.DeserializeObject<Pagination<Review>>(reviews);
            movie.Reviews = reviewsList;
        }
        catch (TimeoutException)
        {
            movie.Reviews = new Pagination<Review>();
        }

        return movie;
    }

    public async Task<Pagination<Movie>> GetListByFilterAsync(MovieFilter filter, CancellationToken cancellationToken)
    {
        if (filter is null)
            throw new ValidationError("Filter is null.");

        if (filter.PageSize > 100)
            throw new ValidationError("Maximum page size is 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await movieRepository.CountByFilterAsync(filter, cancellationToken);

        if (total == 0) return new Pagination<Movie>();

        var paginateResult = await movieRepository.GetListByFilterAsync(filter, cancellationToken);

        var result = new Pagination<Movie>
        {
            PageSize = filter.PageSize,
            Count = total,
            CurrentPage = filter.CurrentPage,
            Result = [.. paginateResult]
        };

        return result;
    }

    public async Task UpdateAsync(int id, Movie movie, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new ValidationError("Id is invalid");

        if (movie == null)
            throw new ValidationError("Movie is null");

        var entity = await movieRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundError(id);

        entity.Title = movie.Title;
        entity.PlotSummary = movie.PlotSummary;
        entity.ReleaseDate = movie.ReleaseDate;
        entity.Genre = movie.Genre;
        entity.Duration = movie.Duration;
        entity.ProductionCompany = movie.ProductionCompany;

        entity.SetUpdatedDate();

        await movieRepository.SaveChangesAsync(cancellationToken);
    }
}
