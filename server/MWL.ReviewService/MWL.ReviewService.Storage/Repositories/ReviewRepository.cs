using Microsoft.EntityFrameworkCore;
using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Models;
using MWL.ReviewService.Domain.Repositories;
using MWL.ReviewService.Storage.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Storage.Repositories
{
    public class ReviewRepository(ReviewContext context) : BaseRepository<Review>(context), IReviewRepository
    {
        public async Task<int> CountByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken)
        {
            var query = Context.Reviews.AsQueryable();

            query = ApplyFilter(filter, query);

            return await query.CountAsync(cancellationToken);
        }

        public Task<Review> GetByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Review>> GetListByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken)
        {
            var query = Context.Reviews.AsQueryable();

            query = ApplyFilter(filter, query);

            query = ApplySorting(filter, query);

            if (filter.CurrentPage > 0)
                query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<Review> ApplySorting(ReviewFilter filter, IQueryable<Review> query)
        {
            query = filter?.OrderBy.ToLower() switch
            {
                "id" => filter.SortBy.Equals("asc", StringComparison.CurrentCultureIgnoreCase)
                    ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                _ => query
            };

            return query;
        }

        private IQueryable<Review> ApplyFilter(ReviewFilter filter, IQueryable<Review> query)
        {
            if (filter.Id > 0)
                query = query.Where(x => x.Id == filter.Id);

            if (filter.MovieId > 0)
                query = query.Where(x => x.MovieId == filter.MovieId);

            return query;
        }

        public async Task DeleteByUserId(string userId, CancellationToken cancellationToken)
        {
            var reviews = await Context.Reviews.Where(x => x.UserId == userId).ToListAsync(cancellationToken);

            Context.Reviews.RemoveRange(reviews);

            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
