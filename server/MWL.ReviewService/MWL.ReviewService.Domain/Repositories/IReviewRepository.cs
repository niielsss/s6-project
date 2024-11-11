using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Domain.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<int> CountByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken);
        Task<Review> GetByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken);
        Task<IEnumerable<Review>> GetListByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken);
        Task DeleteByUserId(string userId, CancellationToken cancellationToken);
    }
}
