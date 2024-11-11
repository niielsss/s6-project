using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Entities.Extensions;
using MWL.ReviewService.Domain.Exceptions;
using MWL.ReviewService.Domain.Models;
using MWL.ReviewService.Domain.Repositories;
using MWL.ReviewService.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Application.Services
{
    public class ReviewService(IReviewRepository reviewRepository) : IReviewService
    {
        public async Task<int> CreateAsync(Review review, CancellationToken cancellationToken)
        {
            if (review is null)
                throw new ValidationError("Review is null");

            review.SetCreatedAt();

            reviewRepository.Add(review);
            await reviewRepository.SaveChangesAsync(cancellationToken);

            return review.Id;
        }

        public async Task DeleteByUserIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ValidationError("Id is invalid");

            await reviewRepository.DeleteByUserId(id, CancellationToken.None);
        }

        public Task<Review> GetByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<Review>> GetListByFilterAsync(ReviewFilter filter, CancellationToken cancellationToken)
        {
            if (filter is null)
                throw new ValidationError("Filter is null");

            if (filter.PageSize > 100)
                throw new ValidationError("Maximum page size is 100");

            if (filter.CurrentPage <= 0) filter.PageSize = 1;

            var total = await reviewRepository.CountByFilterAsync(filter, cancellationToken);

            if (total == 0) return new Pagination<Review>();

            var reviews = await reviewRepository.GetListByFilterAsync(filter, cancellationToken);

            return new Pagination<Review>
            {
                Result = [.. reviews],
                Count = total,
                CurrentPage = filter.CurrentPage,
                PageSize = filter.PageSize
            };
        }

        public async Task<Pagination<Review>> GetListByFilterAsync(ReviewFilter filter)
        {
            if (filter is null)
                throw new ValidationError("Filter is null");

            if (filter.PageSize > 100)
                throw new ValidationError("Maximum page size is 100");

            if (filter.CurrentPage <= 0) filter.PageSize = 1;

            var cancellationToken = new CancellationToken();

            var total = await reviewRepository.CountByFilterAsync(filter, cancellationToken);

            if (total == 0) return new Pagination<Review>();

            var reviews = await reviewRepository.GetListByFilterAsync(filter, cancellationToken);

            return new Pagination<Review>
            {
                Result = [..reviews],
                Count = total,
                CurrentPage = filter.CurrentPage,
                PageSize = filter.PageSize
            };
        }

        public Task UpdateAsync(int id, Review review, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
