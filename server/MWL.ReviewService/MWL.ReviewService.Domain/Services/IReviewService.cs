using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Domain.Services
{
    public interface IReviewService
    {
        Task<Pagination<Review>> GetListByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken);
        Task<Pagination<Review>> GetListByFilterAsync(ReviewFilter filter);
        Task<Review> GetByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken);
        Task<int> CreateAsync(Review review, CancellationToken cancellationToken);
        Task UpdateAsync(int id, Review review, CancellationToken cancellationToken);
        Task DeleteByUserIdAsync(string id, CancellationToken cancellationToken);
    }
}
