using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Services
{
    public interface IMovieService
    {
        Task<Pagination<Movie>> GetListByFilterAsync(MovieFilter filter, CancellationToken cancellationToken);
        Task<Movie> GetByFilterAsync(MovieFilter filter, CancellationToken cancellationToken);
        Task<int> CreateAsync(Movie movie, CancellationToken cancellationToken);
        Task UpdateAsync(int id, Movie movie, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
