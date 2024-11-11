using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<int> CountByFilterAsync(MovieFilter filter, CancellationToken cancellationToken);
        Task<Movie> GetByFilterAsync(MovieFilter filter, CancellationToken cancellationToken);
        Task<IEnumerable<Movie>> GetListByFilterAsync(MovieFilter filter, CancellationToken cancellationToken);
    }
}
