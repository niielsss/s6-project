using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Models;
using MWL.ContentService.Domain.Repositories;
using MWL.ContentService.Storage.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Storage.Repositories
{
    public class MovieRepository(ContentContext context) : BaseRepository<Movie>(context), IMovieRepository
    {
        public async Task<int> CountByFilterAsync(MovieFilter filter, CancellationToken cancellationToken)
        {
            var query = Context.Movies.AsQueryable();

            query = ApplyFilter(filter, query);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<Movie> GetByFilterAsync(MovieFilter filter, CancellationToken cancellationToken)
        {
            var query = Context.Movies.AsQueryable();

            query = ApplyFilter(filter, query);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Movie>> GetListByFilterAsync(MovieFilter filter, CancellationToken cancellationToken)
        {
            var query = Context.Movies.AsQueryable();

            query = ApplyFilter(filter, query);

            query = ApplySorting(filter, query);

            if (filter.CurrentPage > 0)
                query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<Movie> ApplySorting(MovieFilter filter, IQueryable<Movie> query)
        {
            query = filter?.OrderBy.ToLower() switch
            {
                "id" => filter.SortBy.Equals("asc", StringComparison.CurrentCultureIgnoreCase)
                    ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                _ => query
            };

            return query;
        }

        private IQueryable<Movie> ApplyFilter(MovieFilter filter, IQueryable<Movie> query)
        {
            if (filter.Id > 0)
                query = query.Where(x => x.Id == filter.Id);

            return query;
        }
    }
}
